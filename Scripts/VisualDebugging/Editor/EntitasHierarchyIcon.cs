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

using JCMG.EntitasRedux.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	[InitializeOnLoad]
	internal static class EntitasHierarchyIcon
	{
		private static Texture2D ContextHierarchyIcon
		{
			get
			{
				if (_contextHierarchyIcon == null)
				{
					_contextHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasContextHierarchyIcon");
				}

				return _contextHierarchyIcon;
			}
		}

		private static Texture2D ContextErrorHierarchyIcon
		{
			get
			{
				if (_contextErrorHierarchyIcon == null)
				{
					_contextErrorHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasContextErrorHierarchyIcon");
				}

				return _contextErrorHierarchyIcon;
			}
		}

		private static Texture2D EntityHierarchyIcon
		{
			get
			{
				if (_entityHierarchyIcon == null)
				{
					_entityHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasEntityHierarchyIcon");
				}

				return _entityHierarchyIcon;
			}
		}

		private static Texture2D EntityErrorHierarchyIcon
		{
			get
			{
				if (_entityErrorHierarchyIcon == null)
				{
					_entityErrorHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasEntityErrorHierarchyIcon");
				}

				return _entityErrorHierarchyIcon;
			}
		}

		private static Texture2D EntityLinkHierarchyIcon
		{
			get
			{
				if (_entityLinkHierarchyIcon == null)
				{
					_entityLinkHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasEntityLinkHierarchyIcon");
				}

				return _entityLinkHierarchyIcon;
			}
		}

		private static Texture2D EntityLinkWarnHierarchyIcon
		{
			get
			{
				if (_entityLinkWarnHierarchyIcon == null)
				{
					_entityLinkWarnHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasEntityLinkWarnHierarchyIcon");
				}

				return _entityLinkWarnHierarchyIcon;
			}
		}

		private static Texture2D SystemsHierarchyIcon
		{
			get
			{
				if (_systemsHierarchyIcon == null)
				{
					_systemsHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasSystemsHierarchyIcon");
				}

				return _systemsHierarchyIcon;
			}
		}

		private static Texture2D SystemsWarnHierarchyIcon
		{
			get
			{
				if (_systemsWarnHierarchyIcon == null)
				{
					_systemsWarnHierarchyIcon = EditorGUILayoutTools.LoadTexture("l:EntitasSystemsWarnHierarchyIcon");
				}

				return _systemsWarnHierarchyIcon;
			}
		}

		private static Texture2D _contextHierarchyIcon;
		private static Texture2D _contextErrorHierarchyIcon;
		private static Texture2D _entityHierarchyIcon;
		private static Texture2D _entityErrorHierarchyIcon;
		private static Texture2D _entityLinkHierarchyIcon;
		private static Texture2D _entityLinkWarnHierarchyIcon;
		private static Texture2D _systemsHierarchyIcon;
		private static Texture2D _systemsWarnHierarchyIcon;

		static EntitasHierarchyIcon()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
		}

		private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
		{
			var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObject != null)
			{
				const float iconSize = 16f;
				const float iconOffset = iconSize + 2f;
				var rect = new Rect(
					selectionRect.x + selectionRect.width - iconOffset,
					selectionRect.y,
					iconSize,
					iconSize);

				var contextObserver = gameObject.GetComponent<ContextObserverBehaviour>();
				if (contextObserver != null)
				{
					if (contextObserver.ContextObserver.Context.RetainedEntitiesCount != 0)
					{
						GUI.DrawTexture(rect, ContextErrorHierarchyIcon);
					}
					else
					{
						GUI.DrawTexture(rect, ContextHierarchyIcon);
					}

					return;
				}

				var entityBehaviour = gameObject.GetComponent<EntityBehaviour>();
				if (entityBehaviour != null)
				{
					if (entityBehaviour.Entity.IsEnabled)
					{
						GUI.DrawTexture(rect, EntityHierarchyIcon);
					}
					else
					{
						GUI.DrawTexture(rect, EntityErrorHierarchyIcon);
					}

					return;
				}

				var entityLink = gameObject.GetComponent<EntityLink>();
				if (entityLink != null)
				{
					if (entityLink.Entity != null)
					{
						GUI.DrawTexture(rect, EntityLinkHierarchyIcon);
					}
					else
					{
						GUI.DrawTexture(rect, EntityLinkWarnHierarchyIcon);
					}

					return;
				}

				var debugSystemsBehaviour = gameObject.GetComponent<DebugSystemsBehaviour>();
				if (debugSystemsBehaviour != null)
				{
					if (debugSystemsBehaviour.Systems.ExecuteDuration < VisualDebuggingPreferences.SystemWarningThreshold)
					{
						GUI.DrawTexture(rect, SystemsHierarchyIcon);
					}
					else
					{
						GUI.DrawTexture(rect, SystemsWarnHierarchyIcon);
					}
				}
			}
		}
	}
}
