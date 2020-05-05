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
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	public static class EditorGUILayoutTools
	{
		private const int DEFAULT_FOLDOUT_MARGIN = 11;

		public static Texture2D LoadTexture(string label)
		{
			var assets = AssetDatabase.FindAssets(label);
			if (assets.Length != 0)
			{
				var str = assets[0];
				if (str != null)
				{
					return AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(str));
				}
			}

			return null;
		}

		public static bool ObjectFieldButton(string label, string buttonText)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(
				label,
				GUILayout.Width(146f));
			if (buttonText.Length > 24)
			{
				buttonText = "..." + buttonText.Substring(buttonText.Length - 24);
			}

			var num = GUILayout.Button(buttonText, EditorStyles.objectField) ? 1 : 0;
			EditorGUILayout.EndHorizontal();
			return num != 0;
		}

		public static string ObjectFieldOpenFolderPanel(
			string label,
			string buttonText,
			string defaultPath)
		{
			if (!ObjectFieldButton(label, buttonText))
			{
				return null;
			}

			var path = defaultPath ?? "Assets/";
			if (!Directory.Exists(path))
			{
				path = "Assets/";
			}

			return EditorUtility.OpenFolderPanel(label, path, string.Empty)
				.Replace(Directory.GetCurrentDirectory() + "/", string.Empty);
		}

		public static bool MiniButton(string c)
		{
			return MiniButton(c, EditorStyles.miniButton);
		}

		public static bool MiniButtonLeft(string c)
		{
			return MiniButton(c, EditorStyles.miniButtonLeft);
		}

		public static bool MiniButtonMid(string c)
		{
			return MiniButton(c, EditorStyles.miniButtonMid);
		}

		public static bool MiniButtonRight(string c)
		{
			return MiniButton(c, EditorStyles.miniButtonRight);
		}

		private static bool MiniButton(string c, GUIStyle style)
		{
			GUILayoutOption[] guiLayoutOptionArray1;
			if (c.Length != 1)
			{
				guiLayoutOptionArray1 = new GUILayoutOption[0];
			}
			else
			{
				guiLayoutOptionArray1 = new GUILayoutOption[1]
				{
					GUILayout.Width(19f)
				};
			}

			var guiLayoutOptionArray2 = guiLayoutOptionArray1;
			var num = GUILayout.Button(c, style, guiLayoutOptionArray2) ? 1 : 0;
			if (num == 0)
			{
				return num != 0;
			}

			GUI.FocusControl(null);
			return num != 0;
		}

		public static bool Foldout(bool foldout, string content, GUIStyle style, int leftMargin = 11)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(leftMargin);
			foldout = EditorGUILayout.Foldout(foldout, content, style);
			EditorGUILayout.EndHorizontal();
			return foldout;
		}

		public static string SearchTextField(string searchString)
		{
			var changed = GUI.changed;
			GUILayout.BeginHorizontal();
			searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
			if (GUILayout.Button(string.Empty, GUI.skin.FindStyle("ToolbarSeachCancelButton")))
			{
				searchString = string.Empty;
			}

			GUILayout.EndHorizontal();
			GUI.changed = changed;
			return searchString;
		}

		public static bool MatchesSearchString(string str, string search)
		{
			var strArray = search.Split(
				new char[1]
				{
					' '
				},
				StringSplitOptions.RemoveEmptyEntries);
			return strArray.Length == 0 || strArray.Any(str.Contains);
		}

		public static Rect BeginVerticalBox()
		{
			return EditorGUILayout.BeginVertical(GUI.skin.box);
		}

		public static void EndVerticalBox()
		{
			EditorGUILayout.EndVertical();
		}
	}
}
