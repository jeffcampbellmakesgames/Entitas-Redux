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
using System.IO;
using System.Linq;
using Genesis.Shared;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace JCMG.EntitasRedux.Editor
{
	public static class EntityDrawer
	{
		public struct ComponentInfo
		{
			public int index;
			public string name;
			public Type type;
		}

		public static Dictionary<string, bool[]> ContextToUnfoldedComponents
		{
			get
			{
				if (_contextToUnfoldedComponents == null)
				{
					_contextToUnfoldedComponents = new Dictionary<string, bool[]>();
				}

				return _contextToUnfoldedComponents;
			}
		}

		public static Dictionary<string, string[]> ContextToComponentMemberSearch
		{
			get
			{
				if (_contextToComponentMemberSearch == null)
				{
					_contextToComponentMemberSearch = new Dictionary<string, string[]>();
				}

				return _contextToComponentMemberSearch;
			}
		}

		public static Dictionary<string, GUIStyle[]> ContextToColoredBoxStyles
		{
			get
			{
				if (_contextToColoredBoxStyles == null)
				{
					_contextToColoredBoxStyles = new Dictionary<string, GUIStyle[]>();
				}

				return _contextToColoredBoxStyles;
			}
		}

		public static Dictionary<string, ComponentInfo[]> ContextToComponentInfos
		{
			get
			{
				if (_contextToComponentInfos == null)
				{
					_contextToComponentInfos = new Dictionary<string, ComponentInfo[]>();
				}

				return _contextToComponentInfos;
			}
		}

		public static GUIStyle FoldoutStyle
		{
			get
			{
				if (_foldoutStyle == null)
				{
					_foldoutStyle = new GUIStyle(EditorStyles.foldout);
					_foldoutStyle.fontStyle = FontStyle.Bold;
				}

				return _foldoutStyle;
			}
		}

		public static string ComponentNameSearchString
		{
			get
			{
				if (_componentNameSearchString == null)
				{
					_componentNameSearchString = string.Empty;
				}

				return _componentNameSearchString;
			}
			set { _componentNameSearchString = value; }
		}

		private static Dictionary<string, bool[]> _contextToUnfoldedComponents;

		private static Dictionary<string, string[]> _contextToComponentMemberSearch;

		private static Dictionary<string, GUIStyle[]> _contextToColoredBoxStyles;

		private static Dictionary<string, ComponentInfo[]> _contextToComponentInfos;

		private static GUIStyle _foldoutStyle;

		private static string _componentNameSearchString;

		public static readonly IDefaultInstanceCreator[] DEFAULT_INSTANCE_CREATORS;
		public static readonly ITypeDrawer[] TYPE_DRAWERS;
		public static readonly IComponentDrawer[] COMPONENT_DRAWERS;

		static EntityDrawer()
		{
			DEFAULT_INSTANCE_CREATORS =
				ReflectionTools.GetAllImplementingInstancesOfInterface<IDefaultInstanceCreator>().ToArray();
			TYPE_DRAWERS = ReflectionTools.GetAllImplementingInstancesOfInterface<ITypeDrawer>().ToArray();
			COMPONENT_DRAWERS = ReflectionTools.GetAllImplementingInstancesOfInterface<IComponentDrawer>().ToArray();

			EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChanged;
		}

		private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
		{
			ClearCache();
		}

		/// <summary>
		/// Clears all static cached data
		/// </summary>
		private static void ClearCache()
		{
			ContextToUnfoldedComponents.Clear();
			ContextToComponentMemberSearch.Clear();
			ContextToColoredBoxStyles.Clear();
			ContextToComponentInfos.Clear();
		}

		private const string DEFAULT_INSTANCE_CREATOR_TEMPLATE_FORMAT =
			@"using System;
using Entitas.VisualDebugging.Unity.Editor;

public class Default${ShortType}InstanceCreator : IDefaultInstanceCreator {

	public bool HandlesType(Type type) {
		return type == typeof(${Type});
	}

	public object CreateDefault(Type type) {
		// TODO return an instance of type ${Type}
		throw new NotImplementedException();
	}
}
";

		private const string TYPE_DRAWER_TEMPLATE_FORMAT =
			@"using System;
using Entitas;
using Entitas.VisualDebugging.Unity.Editor;

public class ${ShortType}TypeDrawer : ITypeDrawer {

	public bool HandlesType(Type type) {
		return type == typeof(${Type});
	}

	public object DrawAndGetNewValue(Type memberType, string memberName, object value, object target) {
		// TODO draw the type ${Type}
		throw new NotImplementedException();
	}
}
";

		public static void DrawEntity(IEntity entity)
		{
			var bgColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Destroy Entity"))
			{
				entity.Destroy();
			}

			GUI.backgroundColor = bgColor;

			DrawComponents(entity);

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Retained by (" + entity.RetainCount + ")", EditorStyles.boldLabel);

			if (entity.AERC is SafeAERC safeAerc)
			{
				EditorGUILayoutTools.BeginVerticalBox();
				{
					foreach (var owner in safeAerc.Owners.OrderBy(o => o.GetType().Name))
					{
						EditorGUILayout.BeginHorizontal();
						{
							EditorGUILayout.LabelField(owner.ToString());
							if (EditorGUILayoutTools.MiniButton("Release"))
							{
								entity.Release(owner);
							}

							EditorGUILayout.EndHorizontal();
						}
					}
				}
				EditorGUILayoutTools.EndVerticalBox();
			}
		}

		public static void DrawMultipleEntities(IEntity[] entities)
		{
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			{
				var entity = entities[0];
				var index = DrawAddComponentMenu(entity);
				if (index >= 0)
				{
					var componentType = entity.ContextInfo.componentTypes[index];
					foreach (var e in entities)
					{
						var component = e.CreateComponent(index, componentType);
						e.AddComponent(index, component);
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			var bgColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.red;

			if (GUILayout.Button("Destroy selected entities"))
			{
				foreach (var e in entities)
				{
					e.Destroy();
				}
			}

			GUI.backgroundColor = bgColor;

			EditorGUILayout.Space();

			foreach (var e in entities)
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField(e.ToString());

					bgColor = GUI.backgroundColor;
					GUI.backgroundColor = Color.red;

					if (EditorGUILayoutTools.MiniButton("Destroy Entity"))
					{
						e.Destroy();
					}

					GUI.backgroundColor = bgColor;
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		public static void DrawComponents(IEntity entity)
		{
			var unfoldedComponents = GetUnfoldedComponents(entity);
			var componentMemberSearch = GetComponentMemberSearch(entity);

			EditorGUILayoutTools.BeginVerticalBox();

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Components (" + entity.GetComponents().Length + ")", EditorStyles.boldLabel);
				if (EditorGUILayoutTools.MiniButtonLeft("▸"))
				{
					for (var i = 0; i < unfoldedComponents.Length; i++)
					{
						unfoldedComponents[i] = false;
					}
				}

				if (EditorGUILayoutTools.MiniButtonRight("▾"))
				{
					for (var i = 0; i < unfoldedComponents.Length; i++)
					{
						unfoldedComponents[i] = true;
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			var index = DrawAddComponentMenu(entity);
			if (index >= 0)
			{
				var componentType = entity.ContextInfo.componentTypes[index];
				var component = entity.CreateComponent(index, componentType);
				entity.AddComponent(index, component);
			}

			EditorGUILayout.Space();

			ComponentNameSearchString = EditorGUILayoutTools.SearchTextField(ComponentNameSearchString);

			EditorGUILayout.Space();

			var indices = entity.GetComponentIndices();
			var components = entity.GetComponents();
			for (var i = 0; i < components.Length; i++)
			{
				DrawComponent(
					unfoldedComponents,
					componentMemberSearch,
					entity,
					indices[i],
					components[i]);
			}

			EditorGUILayoutTools.EndVerticalBox();
		}

		public static void DrawComponent(bool[] unfoldedComponents,
										 string[] componentMemberSearch,
										 IEntity entity,
										 int index,
										 IComponent component)
		{
			var componentType = component.GetType();
			var componentName = componentType.Name.RemoveComponentSuffix();
			if (EditorGUILayoutTools.MatchesSearchString(componentName.ToLower(), ComponentNameSearchString.ToLower()))
			{
				var boxStyle = GetColoredBoxStyle(entity, index);

				EditorGUILayout.BeginVertical(boxStyle);

				if (!Attribute.IsDefined(componentType, typeof(DontDrawComponentAttribute)))
				{
					var memberInfos = componentType.GetPublicMemberInfos();
					EditorGUILayout.BeginHorizontal();
					{
						if (memberInfos.Count == 0)
						{
							EditorGUILayout.LabelField(componentName, EditorStyles.boldLabel);
						}
						else
						{
							unfoldedComponents[index] = EditorGUILayoutTools.Foldout(
								unfoldedComponents[index],
								componentName,
								FoldoutStyle);
							if (unfoldedComponents[index])
							{
								componentMemberSearch[index] = memberInfos.Count > 5
									? EditorGUILayoutTools.SearchTextField(componentMemberSearch[index])
									: string.Empty;
							}
						}

						if (EditorGUILayoutTools.MiniButton("-"))
						{
							entity.RemoveComponent(index);
						}
					}
					EditorGUILayout.EndHorizontal();

					if (unfoldedComponents[index])
					{
						var newComponent = entity.CreateComponent(index, componentType);
						component.CopyPublicMemberValues(newComponent);

						var changed = false;
						var componentDrawer = GetComponentDrawer(componentType);
						if (componentDrawer != null)
						{
							EditorGUI.BeginChangeCheck();
							{
								componentDrawer.DrawComponent(newComponent);
							}
							changed = EditorGUI.EndChangeCheck();
						}
						else
						{
							foreach (var info in memberInfos)
							{
								if (EditorGUILayoutTools.MatchesSearchString(
									info.name.ToLower(),
									componentMemberSearch[index].ToLower()))
								{
									var memberValue = info.GetValue(newComponent);
									var memberType = memberValue == null ? info.type : memberValue.GetType();
									if (DrawObjectMember(
										memberType,
										info.name,
										memberValue,
										newComponent,
										info.SetValue))
									{
										changed = true;
									}
								}
							}
						}

						if (changed)
						{
							entity.ReplaceComponent(index, newComponent);
						}
						else
						{
							entity.GetComponentPool(index).Push(newComponent);
						}
					}
				}
				else
				{
					EditorGUILayout.LabelField(componentName, "[DontDrawComponent]", EditorStyles.boldLabel);
				}

				EditorGUILayoutTools.EndVerticalBox();
			}
		}

		public static void DrawComponent(
			bool[] unfoldedComponents,
			string contextName,
			int totalNumberOfComponents,
			int index,
			IComponent component,
			Action<IComponent> removeComponent,
			Action onComponentChanged)
		{
			var componentType = component.GetType();
			var componentName = componentType.Name.RemoveComponentSuffix();
			var boxStyle = GetColoredBoxStyle(contextName, totalNumberOfComponents, index);

			EditorGUILayout.BeginVertical(boxStyle);

			if (!Attribute.IsDefined(componentType, typeof(DontDrawComponentAttribute)))
			{
				var memberInfos = componentType.GetPublicMemberInfos();
				EditorGUILayout.BeginHorizontal();
				{
					if (memberInfos.Count == 0)
					{
						EditorGUILayout.LabelField(componentName, EditorStyles.boldLabel);
					}
					else
					{
						unfoldedComponents[index] = EditorGUILayoutTools.Foldout(
							unfoldedComponents[index],
							componentName,
							FoldoutStyle);
					}

					if (EditorGUILayoutTools.MiniButton("-"))
					{
						removeComponent?.Invoke(component);
					}
				}
				EditorGUILayout.EndHorizontal();

				if (unfoldedComponents[index])
				{
					var changed = false;
					var componentDrawer = GetComponentDrawer(componentType);
					if (componentDrawer != null)
					{
						EditorGUI.BeginChangeCheck();
						{
							componentDrawer.DrawComponent(component);
						}
						changed = EditorGUI.EndChangeCheck();
					}
					else
					{
						foreach (var info in memberInfos)
						{
							var memberValue = info.GetValue(component);
							var memberType = memberValue == null ? info.type : memberValue.GetType();
							if (DrawObjectMember(
								memberType,
								info.name,
								memberValue,
								component,
								info.SetValue))
							{
								changed = true;
							}

						}
					}

					if (changed)
					{
						onComponentChanged?.Invoke();
					}
				}
			}
			else
			{
				EditorGUILayout.LabelField(componentName, "[DontDrawComponent]", EditorStyles.boldLabel);
			}

			EditorGUILayoutTools.EndVerticalBox();

		}

		public static bool DrawObjectMember(Type memberType, string memberName, object value, object target,
											Action<object, object> setValue)
		{
			if (value == null)
			{
				EditorGUI.BeginChangeCheck();
				{
					var isUnityObject = memberType == typeof(Object) || memberType.IsSubclassOf(typeof(Object));
					EditorGUILayout.BeginHorizontal();
					{
						if (isUnityObject)
						{
							setValue(
								target,
								EditorGUILayout.ObjectField(
									memberName,
									(Object)value,
									memberType,
									true));
						}
						else
						{
							EditorGUILayout.LabelField(memberName, "null");
						}

						if (EditorGUILayoutTools.MiniButton("new " + memberType.ToCompilableString().ShortTypeName()))
						{
							if (CreateDefault(memberType, out var defaultValue))
							{
								setValue(target, defaultValue);
							}
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				return EditorGUI.EndChangeCheck();
			}

			if (!memberType.IsValueType)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
			}

			EditorGUI.BeginChangeCheck();
			{
				var typeDrawer = GetTypeDrawer(memberType);
				if (typeDrawer != null)
				{
					var newValue = typeDrawer.DrawAndGetNewValue(
						memberType,
						memberName,
						value,
						target);
					setValue(target, newValue);
				}
				else
				{
					var targetType = target.GetType();
					var shouldDraw = !targetType.ImplementsInterface<IComponent>() ||
									 !Attribute.IsDefined(targetType, typeof(DontDrawComponentAttribute));
					if (shouldDraw)
					{
						EditorGUILayout.LabelField(memberName, value.ToString());

						var indent = EditorGUI.indentLevel;
						EditorGUI.indentLevel += 1;

						EditorGUILayout.BeginVertical();
						{
							foreach (var info in memberType.GetPublicMemberInfos())
							{
								var mValue = info.GetValue(value);
								var mType = mValue == null ? info.type : mValue.GetType();
								DrawObjectMember(
									mType,
									info.name,
									mValue,
									value,
									info.SetValue);
								if (memberType.IsValueType)
								{
									setValue(target, value);
								}
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUI.indentLevel = indent;
					}
					else
					{
						DrawUnsupportedType(memberType, memberName, value);
					}
				}

				if (!memberType.IsValueType)
				{
					EditorGUILayout.EndVertical();
					if (EditorGUILayoutTools.MiniButton("×"))
					{
						setValue(target, null);
					}

					EditorGUILayout.EndHorizontal();
				}
			}

			return EditorGUI.EndChangeCheck();
		}

		public static bool CreateDefault(Type type, out object defaultValue)
		{
			try
			{
				defaultValue = Activator.CreateInstance(type);
				return true;
			}
			catch (Exception)
			{
				foreach (var creator in DEFAULT_INSTANCE_CREATORS)
				{
					if (creator.HandlesType(type))
					{
						defaultValue = creator.CreateDefault(type);
						return true;
					}
				}
			}

			defaultValue = null;
			return false;
		}

		private static int DrawAddComponentMenu(IEntity entity)
		{
			var componentInfos = GetComponentInfos(entity)
				.Where(info => !entity.HasComponent(info.index))
				.ToArray();
			var componentNames = componentInfos
				.Select(info => info.name)
				.ToArray();
			var index = EditorGUILayout.Popup("Add Component", -1, componentNames);
			if (index >= 0)
			{
				return componentInfos[index].index;
			}

			return -1;
		}

		private static void DrawUnsupportedType(Type memberType, string memberName, object value)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(memberName, value.ToString());
				EditorGUILayout.HelpBox("Missing ITypeDrawer", MessageType.Warning);
			}
			EditorGUILayout.EndHorizontal();
		}

		private static void GenerateTemplate(string folder, string filePath, string template)
		{
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			File.WriteAllText(filePath, template);
			EditorApplication.isPlaying = false;
			AssetDatabase.Refresh();
			Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(filePath);
		}

		internal static bool[] GetUnfoldedComponents(IEntity entity)
		{
			return GetUnfoldedComponents(entity.ContextInfo.name, entity.TotalComponents);
		}

		internal static bool[] GetUnfoldedComponents(string contextName, int totalNumberOfComponents)
		{
			if (!ContextToUnfoldedComponents.TryGetValue(contextName, out var unfoldedComponents))
			{
				unfoldedComponents = new bool[totalNumberOfComponents];
				for (var i = 0; i < unfoldedComponents.Length; i++)
				{
					unfoldedComponents[i] = true;
				}

				ContextToUnfoldedComponents.Add(contextName, unfoldedComponents);
			}

			return unfoldedComponents;
		}

		internal static string[] GetComponentMemberSearch(IEntity entity)
		{
			return GetComponentMemberSearch(entity.ContextInfo.name, entity.TotalComponents);
		}

		internal static string[] GetComponentMemberSearch(string contextName, int totalNumberOfComponents)
		{
			if (!ContextToComponentMemberSearch.TryGetValue(contextName, out var componentMemberSearch))
			{
				componentMemberSearch = new string[totalNumberOfComponents];
				for (var i = 0; i < componentMemberSearch.Length; i++)
				{
					componentMemberSearch[i] = string.Empty;
				}

				ContextToComponentMemberSearch.Add(contextName, componentMemberSearch);
			}

			return componentMemberSearch;
		}

		internal static ComponentInfo[] GetComponentInfos(IEntity entity)
		{
			if (!ContextToComponentInfos.TryGetValue(entity.ContextInfo.name, out var infos))
			{
				var contextInfo = entity.ContextInfo;
				var infosList = new List<ComponentInfo>(contextInfo.componentTypes.Length);
				for (var i = 0; i < contextInfo.componentTypes.Length; i++)
				{
					infosList.Add(
						new ComponentInfo
						{
							index = i, name = contextInfo.componentNames[i], type = contextInfo.componentTypes[i]
						});
				}

				infos = infosList.ToArray();
				ContextToComponentInfos.Add(entity.ContextInfo.name, infos);
			}

			return infos;
		}

		internal static ComponentInfo[] GetComponentInfos(string contextName, string[] componentNames, Type[] componentTypes)
		{
			if (!ContextToComponentInfos.TryGetValue(contextName, out var infos))
			{
				var infosList = new List<ComponentInfo>(componentTypes.Length);
				for (var i = 0; i < componentTypes.Length; i++)
				{
					infosList.Add(
						new ComponentInfo
						{
							index = i, name = componentNames[i], type = componentTypes[i]
						});
				}

				infos = infosList.ToArray();
				ContextToComponentInfos.Add(contextName, infos);
			}

			return infos;
		}

		internal static GUIStyle GetColoredBoxStyle(IEntity entity, int index)
		{
			return GetColoredBoxStyle(entity.ContextInfo.name, entity.TotalComponents, index);
		}

		internal static GUIStyle GetColoredBoxStyle(string contextName, int totalNumberOfComponents, int index)
		{
			if (!ContextToColoredBoxStyles.TryGetValue(contextName, out var styles))
			{
				styles = new GUIStyle[totalNumberOfComponents];
				for (var i = 0; i < styles.Length; i++)
				{
					var hue = i / (float)totalNumberOfComponents;
					var componentColor = Color.HSVToRGB(hue, 0.7f, 1f);
					componentColor.a = 0.15f;

					var tex = CreateTexture(2, 2, componentColor);

					var style = new GUIStyle(GUI.skin.box);
					style.normal.background = tex;
					style.normal.scaledBackgrounds = new[]
					{
						tex
					};
					style.onNormal.background = tex;
					style.onNormal.scaledBackgrounds = new[]
					{
						tex
					};

					style.active.background = tex;
					style.active.scaledBackgrounds = new[]
					{
						tex
					};
					style.onActive.background = tex;
					style.onActive.scaledBackgrounds = new[]
					{
						tex
					};

					style.focused.background = tex;
					style.focused.scaledBackgrounds = new[]
					{
						tex
					};
					style.onFocused.background = tex;
					style.onFocused.scaledBackgrounds = new[]
					{
						tex
					};

					style.hover.background = tex;
					style.hover.scaledBackgrounds = new[]
					{
						tex
					};
					style.onHover.background = tex;
					style.onHover.scaledBackgrounds = new[]
					{
						tex
					};

					styles[i] = style;
				}

				ContextToColoredBoxStyles.Add(contextName, styles);
			}

			return styles[index];
		}

		private static Texture2D CreateTexture(int width, int height, Color color)
		{
			var pixels = new Color[width * height];
			for (var i = 0; i < pixels.Length; ++i)
			{
				pixels[i] = color;
			}

			var result = new Texture2D(width, height);
			result.SetPixels(pixels);
			result.Apply();
			return result;
		}

		private static IComponentDrawer GetComponentDrawer(Type type)
		{
			foreach (var drawer in COMPONENT_DRAWERS)
			{
				if (drawer.HandlesType(type))
				{
					return drawer;
				}
			}

			return null;
		}

		private static ITypeDrawer GetTypeDrawer(Type type)
		{
			foreach (var drawer in TYPE_DRAWERS)
			{
				if (drawer.HandlesType(type))
				{
					return drawer;
				}
			}

			return null;
		}
	}
}
