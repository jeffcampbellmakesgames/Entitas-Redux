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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Genesis.Shared;
using JCMG.EntitasRedux.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.Blueprints.Editor
{
	public abstract class BlueprintInspector : UnityEditor.Editor
	{
		/// <summary>
		/// Returns true if the context has any valid components that can be selected in the inspector, otherwise false.
		/// </summary>
		protected bool ContextHasSerializableComponents => _componentTypes.Length > 0 ||
														 _componentDisplayNames.Length > 0 ||
														 _componentInfo.Length > 0;

		private static readonly IEnumerable<Assembly> ASSEMBLIES;

		private int _selectedComponentIndex;
		private Type _targetType;
		private string _contextName;
		private Type[] _componentTypes;
		private string[] _componentDisplayNames;
		private int _totalNumberOfComponents;
		private EntityDrawer.ComponentInfo[] _componentInfo;

		// UI
		private const string ADD_BUTTON_TEXT = "Add";
		private const string NO_VALID_COMPONENTS_WARNING =
			"There are not any serializable components with a default constructor for the context [{0}]. Only components " +
			"that meet that criteria will be available for blueprint usage.";

		static BlueprintInspector()
		{
			ASSEMBLIES = ReflectionTools.GetAvailableAssemblies();
		}

		protected virtual void OnEnable()
		{
			// Get blueprint type info
			_targetType = target.GetType();
			_contextName = GetContextNameFromBlueprintType(_targetType);

			// Get all possible serializable components info for the context this blueprint is associated with
			GetComponentInfo(
				_contextName,
				out _componentTypes,
				out _componentDisplayNames,
				out _componentInfo,
				out _totalNumberOfComponents);
		}

		protected void DrawComponentArrayGUI(List<IComponent> components)
		{
			var unfoldedComponents = EntityDrawer.GetUnfoldedComponents(_contextName, _totalNumberOfComponents);
			for (var i = 0; i < components.Count; i++)
			{
				var index = GetComponentIndex(components[i]);
				if (components[i] == null || index == -1)
				{
					continue;
				}

				EntityDrawer.DrawComponent(
					unfoldedComponents,
					_contextName,
					_totalNumberOfComponents,
					index,
					components[i],
					OnComponentRemoved,
					OnComponentChanged);
			}
		}

		protected void DrawComponentSelectorGUI(List<IComponent> components, Action onComponentAdded)
		{
			if (!ContextHasSerializableComponents)
			{
				EditorGUILayout.HelpBox(string.Format(NO_VALID_COMPONENTS_WARNING, _contextName), MessageType.Warning);
			}
			else
			{
				using (new EditorGUILayout.HorizontalScope())
				{
					// Select a component type specific to this blueprint's context.
					_selectedComponentIndex = EditorGUILayout.Popup(_selectedComponentIndex, _componentDisplayNames);

					// Add component if not already present in the blueprint.
					using (new EditorGUI.DisabledScope(
						HasComponentType(
							components,
							_componentTypes[_selectedComponentIndex])))
					{
						if (GUILayout.Button(ADD_BUTTON_TEXT))
						{
							var componentInstance = Activator.CreateInstance(_componentTypes[_selectedComponentIndex]);
							components.Add((IComponent)componentInstance);

							onComponentAdded?.Invoke();
						}
					}
				}
			}
		}

		/// <summary>
		/// Retrieves the component index for <paramref name="component"/> based on its type and presence in the context's
		/// component lookup. If a valid index cannot be found, an assertion is thrown.
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		protected int GetComponentIndex(IComponent component)
		{
			var result = -1;
			var componentType = component.GetType();
			for (var i = 0; i < _componentInfo.Length; i++)
			{
				if (componentType != _componentInfo[i].type)
				{
					continue;
				}

				result = _componentInfo[i].index;
			}

			return result;
		}

		/// <summary>
		/// Returns true if a <see cref="IComponent"/> instance of <paramref name="componentType"/> exists in
		/// <paramref name="components"/>, otherwise false.
		/// </summary>
		/// <param name="components"></param>
		/// <param name="componentType"></param>
		/// <returns></returns>
		protected bool HasComponentType(List<IComponent> components, Type componentType)
		{
			var result = false;
			for (var i = 0; i < components.Count; i++)
			{
				if (components[i].GetType() != componentType)
				{
					continue;
				}

				result = true;
				break;
			}

			return result;
		}

		protected void GetComponentInfo(string contextTypeName,
										out Type[] componentTypes,
										out string[] componentDisplayNames,
										out EntityDrawer.ComponentInfo[] componentInfo,
										out int totalNumberOfComponents)
		{
			// Get static component lookup type for the context this blueprint is associated with
			var componentLookupTypeName = GetContextLookupTypeNameFromContextName(contextTypeName);
			var componentLookupType = GetTypeByName(componentLookupTypeName);

			const string COMPONENT_NAMES_MEMBER_NAME = "ComponentNames";
			const string COMPONENT_TYPES_MEMBER_NAME = "ComponentTypes";

			// Get component names
			var componentNamesMemberInfo = componentLookupType.GetField(
				COMPONENT_NAMES_MEMBER_NAME,
				BindingFlags.Public | BindingFlags.Static);
			var allComponentDisplayNames = (string[])componentNamesMemberInfo.GetValue(null);

			// Get component types
			var componentTypesMemberInfo = componentLookupType.GetField(
				COMPONENT_TYPES_MEMBER_NAME,
				BindingFlags.Public | BindingFlags.Static);
			var allComponentTypes = (Type[])componentTypesMemberInfo.GetValue(null);

			// Regardless of how many components we end up filtering down to for a developer to be able to add
			// the total number of components always reflects the total for the context.
			totalNumberOfComponents = allComponentTypes.Length;

			// Filter down these lists to only those components that we can instantiate and serialize
			var componentTypesList = new List<Type>();
			var componentDisplayNamesList = new List<string>();
			var componentInfoList = new List<EntityDrawer.ComponentInfo>();
			for (var i = 0; i < allComponentTypes.Length; i++)
			{
				// If our component type is not serializable OR does not provide a default constructor, don't enable
				// selection of it.
				if (!allComponentTypes[i].IsSerializable ||
					allComponentTypes[i].GetConstructor(Type.EmptyTypes) == null)
				{
					continue;
				}

				componentTypesList.Add(allComponentTypes[i]);
				componentDisplayNamesList.Add(allComponentDisplayNames[i]);
				componentInfoList.Add(new EntityDrawer.ComponentInfo()
				{
					index = i,
					name = allComponentDisplayNames[i],
					type = allComponentTypes[i]
				});
			}

			componentTypes = componentTypesList.ToArray();
			componentDisplayNames = componentDisplayNamesList.ToArray();
			componentInfo = componentInfoList.ToArray();
		}

		protected string GetContextNameFromBlueprintType(Type type)
		{
			const string BLUEPRINT_BEHAVIOR_SUFFIX = "BlueprintBehaviour";
			const string BLUEPRINT_SUFFIX = "Blueprint";

			return type.Name
					.Replace(BLUEPRINT_BEHAVIOR_SUFFIX, string.Empty)
					.Replace(BLUEPRINT_SUFFIX, string.Empty);
		}

		protected string GetContextLookupTypeNameFromContextName(string contextName)
		{
			const string COMPONENTS_LOOKUP_SUFFIX = "ComponentsLookup";

			return contextName + COMPONENTS_LOOKUP_SUFFIX;
		}

		protected Type GetTypeByName(string typeName)
		{
			return ASSEMBLIES.SelectMany(x => x.GetTypes()).FirstOrDefault(y => y.Name == typeName);
		}

		protected abstract void OnComponentRemoved(IComponent component);

		protected void OnComponentChanged()
		{
			EditorUtility.SetDirty(target);
		}
	}
}
