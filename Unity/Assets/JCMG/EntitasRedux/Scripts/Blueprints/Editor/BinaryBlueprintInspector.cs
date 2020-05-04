/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Linq;
using JCMG.EntitasRedux.VisualDebugging.Editor;
using JCMG.Genesis.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.Blueprints.Editor
{
	[CustomEditor(typeof(BinaryBlueprint))]
	public class BinaryBlueprintInspector : UnityEditor.Editor
	{
		private string[] _allContextNames;

		private IContext[] _allContexts;

		private Blueprint _blueprint;

		private IContext _context;
		private int _contextIndex;
		private IEntity _entity;

		public static BinaryBlueprint[] FindAllBlueprints()
		{
			return new BinaryBlueprint[0];

			// TODO Rework or Orphan Blueprints to side branch
			//return AssetDatabase.FindAssets("l:" + BinaryBlueprintPostprocessor.ASSET_LABEL)
			//	.Select(AssetDatabase.GUIDToAssetPath)
			//	.Select(AssetDatabase.LoadAssetAtPath<BinaryBlueprint>)
			//	.ToArray();
		}

		// TODO Rework or Orphan Blueprints to side branch
		//[DidReloadScripts]
		[MenuItem("Tools/JCMG/EntitasRedux/Blueprints/Update all Blueprints", false, 300)]
		public static void UpdateAllBinaryBlueprints()
		{
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				var allContexts = FindAllContexts();
				if (allContexts == null)
				{
					return;
				}

				var binaryBlueprints = FindAllBlueprints();
				var allContextNames = allContexts.Select(context => context.ContextInfo.name).ToArray();
				var updated = 0;
				foreach (var binaryBlueprint in binaryBlueprints)
				{
					var didUpdate = UpdateBinaryBlueprint(binaryBlueprint, allContexts, allContextNames);
					if (didUpdate)
					{
						updated += 1;
					}
				}

				if (updated > 0)
				{
					Debug.Log("Validated " + binaryBlueprints.Length + " Blueprints, " + updated + " have been updated.");
				}
			}
		}

		public static bool UpdateBinaryBlueprint(BinaryBlueprint binaryBlueprint, IContext[] allContexts,
		                                         string[] allContextNames)
		{
			var blueprint = binaryBlueprint.Deserialize();
			var needsUpdate = false;

			var contextIndex = Array.IndexOf(allContextNames, blueprint.contextIdentifier);
			if (contextIndex < 0)
			{
				contextIndex = 0;
				needsUpdate = true;
			}

			var context = allContexts[contextIndex];
			blueprint.contextIdentifier = context.ContextInfo.name;

			foreach (var component in blueprint.components)
			{
				var type = component.fullTypeName.ToType();
				var index = Array.IndexOf(context.ContextInfo.componentTypes, type);

				if (index != component.index)
				{
					Debug.Log(
						string.Format(
							"Blueprint '{0}' has invalid or outdated component index for '{1}'. Index was {2} but should be {3}. Updated index.",
							blueprint.name,
							component.fullTypeName,
							component.index,
							index));

					component.index = index;
					needsUpdate = true;
				}
			}

			if (needsUpdate)
			{
				Debug.Log("Updating Blueprint '" + blueprint.name + "'");
				binaryBlueprint.Serialize(blueprint);
			}

			return needsUpdate;
		}

		private static IContext[] FindAllContexts()
		{
			var contextsType = ReflectionTools.GetAllImplementingTypesOfInterface<IContext>()
				.SingleOrDefault();
			if (contextsType != null)
			{
				var contexts = (IContexts)Activator.CreateInstance(contextsType);
				return contexts.AllContexts;
			}

			return null;
		}

		private void Awake()
		{
			_allContexts = FindAllContexts();
			if (_allContexts == null)
			{
				return;
			}

			var binaryBlueprint = (BinaryBlueprint)target;

			_allContextNames = _allContexts.Select(context => context.ContextInfo.name).ToArray();

			UpdateBinaryBlueprint(binaryBlueprint, _allContexts, _allContextNames);

			_blueprint = binaryBlueprint.Deserialize();

			AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), _blueprint.name);

			_contextIndex = Array.IndexOf(_allContextNames, _blueprint.contextIdentifier);
			SwitchToContext();

			_entity.ApplyBlueprint(_blueprint);

			// Serialize in case the structure of a component changed, e.g. field got removed
			binaryBlueprint.Serialize(_entity);
		}

		private void OnDisable()
		{
			if (_context != null)
			{
				_context.Reset();
			}
		}

		public override void OnInspectorGUI()
		{
			var binaryBlueprint = (BinaryBlueprint)target;

			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.LabelField("Blueprint", EditorStyles.boldLabel);
				binaryBlueprint.name = EditorGUILayout.TextField("Name", binaryBlueprint.name);

				if (_context != null)
				{
					EditorGUILayout.BeginHorizontal();
					{
						_contextIndex = EditorGUILayout.Popup(_contextIndex, _allContextNames);

						if (EditorGUILayoutTools.MiniButton("Switch Context"))
						{
							SwitchToContext();
						}
					}
					EditorGUILayout.EndHorizontal();

					EntityDrawer.DrawComponents(_entity);
				}
				else
				{
					EditorGUILayout.LabelField("No contexts found!");
				}
			}
			var changed = EditorGUI.EndChangeCheck();
			if (changed)
			{
				binaryBlueprint.Serialize(_entity);
				AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), binaryBlueprint.name);
				EditorUtility.SetDirty(target);
			}
		}

		private void SwitchToContext()
		{
			if (_context != null)
			{
				_context.Reset();
			}

			var targetContext = _allContexts[_contextIndex];
			_context = (IContext)Activator.CreateInstance(targetContext.GetType());
			_entity = (IEntity)_context.GetType().GetMethod("CreateEntity").Invoke(_context, null);
		}
	}
}
