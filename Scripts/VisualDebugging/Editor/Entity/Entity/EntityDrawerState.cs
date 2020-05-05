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
using JCMG.Genesis.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	public static partial class EntityDrawer
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
		}

		private static bool[] GetUnfoldedComponents(IEntity entity)
		{
			if (!ContextToUnfoldedComponents.TryGetValue(entity.ContextInfo.name, out var unfoldedComponents))
			{
				unfoldedComponents = new bool[entity.TotalComponents];
				for (var i = 0; i < unfoldedComponents.Length; i++)
				{
					unfoldedComponents[i] = true;
				}

				ContextToUnfoldedComponents.Add(entity.ContextInfo.name, unfoldedComponents);
			}

			return unfoldedComponents;
		}

		private static string[] GetComponentMemberSearch(IEntity entity)
		{
			if (!ContextToComponentMemberSearch.TryGetValue(entity.ContextInfo.name, out var componentMemberSearch))
			{
				componentMemberSearch = new string[entity.TotalComponents];
				for (var i = 0; i < componentMemberSearch.Length; i++)
				{
					componentMemberSearch[i] = string.Empty;
				}

				ContextToComponentMemberSearch.Add(entity.ContextInfo.name, componentMemberSearch);
			}

			return componentMemberSearch;
		}

		private static ComponentInfo[] GetComponentInfos(IEntity entity)
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

		private static GUIStyle GetColoredBoxStyle(IEntity entity, int index)
		{
			if (!ContextToColoredBoxStyles.TryGetValue(entity.ContextInfo.name, out var styles))
			{
				styles = new GUIStyle[entity.TotalComponents];
				for (var i = 0; i < styles.Length; i++)
				{
					var hue = i / (float)entity.TotalComponents;
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

				ContextToColoredBoxStyles.Add(entity.ContextInfo.name, styles);
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
