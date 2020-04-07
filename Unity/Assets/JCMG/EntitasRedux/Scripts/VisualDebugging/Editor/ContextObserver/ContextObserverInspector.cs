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

using System.Linq;
using JCMG.Genesis.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	[CustomEditor(typeof(ContextObserverBehaviour))]
	internal sealed class ContextObserverInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var contextObserver = ((ContextObserverBehaviour)target).ContextObserver;

			EditorGUILayoutTools.BeginSectionContent();
			{
				EditorGUILayout.LabelField(contextObserver.Context.ContextInfo.name, EditorStyles.boldLabel);
				EditorGUILayout.LabelField("Entities", contextObserver.Context.Count.ToString());
				EditorGUILayout.LabelField("Reusable entities", contextObserver.Context.ReusableEntitiesCount.ToString());

				var retainedEntitiesCount = contextObserver.Context.RetainedEntitiesCount;
				if (retainedEntitiesCount != 0)
				{
					var c = GUI.color;
					GUI.color = Color.red;
					EditorGUILayout.LabelField("Retained entities", retainedEntitiesCount.ToString());
					GUI.color = c;
					EditorGUILayout.HelpBox(
						"WARNING: There are retained entities.\nDid you call entity.Retain(owner) and forgot to call entity.Release(owner)?",
						MessageType.Warning);
				}

				EditorGUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Create Entity"))
					{
						var entity = contextObserver.Context.CreateEntity();
						var entityBehaviour = FindObjectsOfType<EntityBehaviour>()
							.Single(eb => eb.Entity == entity);

						Selection.activeGameObject = entityBehaviour.gameObject;
					}

					var bgColor = GUI.backgroundColor;
					GUI.backgroundColor = Color.red;
					if (GUILayout.Button("Destroy All Entities"))
					{
						contextObserver.Context.DestroyAllEntities();
					}

					GUI.backgroundColor = bgColor;
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayoutTools.EndSectionContent();

			var groups = contextObserver.Groups;
			if (groups.Length != 0)
			{
				EditorGUILayoutTools.BeginSectionContent();
				{
					EditorGUILayout.LabelField("Groups (" + groups.Length + ")", EditorStyles.boldLabel);
					foreach (var group in groups.OrderByDescending(g => g.Count))
					{
						EditorGUILayout.BeginHorizontal();
						{
							EditorGUILayout.LabelField(group.ToString());
							EditorGUILayout.LabelField(group.Count.ToString(), GUILayout.Width(48));
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayoutTools.EndSectionContent();
			}

			EditorUtility.SetDirty(target);
		}
	}
}
