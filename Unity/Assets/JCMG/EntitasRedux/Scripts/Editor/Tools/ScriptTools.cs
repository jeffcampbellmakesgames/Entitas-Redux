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

using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Helper methods for creating scripts from template files.
	/// </summary>
	public static class ScriptTools
	{
		// ReSharper disable once ClassNeverInstantiated.Local
		private class DoCreateScriptAsset : EndNameEditAction
		{
			// Script replace tokens
			private const string CLASS_NAME_TOKEN = "#SCRIPTNAME#";
			private const string NAMESPACE_TOKEN = "#NAMESPACE#";

			private const string EMPTY_SPACE_STR = " ";
			private const string FORWARD_SLASH_STR = "/";
			private const string PERIOD_STR = ".";

			private static readonly char[] TRIM_END_CHARS;

			static DoCreateScriptAsset()
			{
				TRIM_END_CHARS = new[]
				{
					'/',
					'.'
				};
			}

			public override void Action(int instanceID, string pathName, string resourceFile)
			{
				var text = File.ReadAllText(resourceFile);
				var fileName = Path.GetFileName(pathName);
				var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
				var filePath = pathName.Replace(fileName, string.Empty);

				// Attempt to replace class name
				var className = fileNameWithoutExtension.Replace(EMPTY_SPACE_STR, string.Empty);
				text = text.Replace(CLASS_NAME_TOKEN, className);

				// Attempt to replace namespace
				var namespaceValue = filePath
					.Replace(FORWARD_SLASH_STR, PERIOD_STR)
					.TrimEnd(TRIM_END_CHARS);
				text = text.Replace(NAMESPACE_TOKEN, namespaceValue);

				// Create the script asset
				File.WriteAllText(pathName, text);
				AssetDatabase.ImportAsset(pathName);

				// Show the newly created script asset in the project folder
				var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
				ProjectWindowUtil.ShowCreatedAsset(asset);
			}
		}

		private const string SCRIPT_ICON_NAME = "cs Script Icon";
		private const string NEW_SCRIPT_FILENAME_FORMAT = "New{0}";
		private const string TXT_EXTENSION = ".txt";
		private const string CS_EXTENSION = ".cs";

		/// <summary>
		/// Creates a new CSharp script based off a text file named <paramref name="fileName"/> located in a Unity
		/// folder whose meta <see cref="GUID"/> is <paramref name="templateFolderGUID"/>.
		/// </summary>
		/// <param name="templateFolderGUID">The Unity meta <see cref="GUID"/> value of the folder where the
		/// <paramref name="fileName"/> resides.</param>
		/// <param name="fileName">The name of the script file with .txt extension</param>
		public static void CreateScriptAsset(string templateFolderGUID, string fileName)
		{
			// Construct script path and initial name
			var scriptTemplateFolderPath = AssetDatabase.GUIDToAssetPath(templateFolderGUID);
			var scriptPath = Path.Combine(scriptTemplateFolderPath, fileName);
			var csIcon = EditorGUIUtility.IconContent(SCRIPT_ICON_NAME).image as Texture2D;
			var newFileName = string.Format(NEW_SCRIPT_FILENAME_FORMAT, fileName.Replace(TXT_EXTENSION, CS_EXTENSION));

			// Create and resolve script asset
			var endNameAction = ScriptableObject.CreateInstance<DoCreateScriptAsset>();
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
				0,
				endNameAction,
				newFileName,
				csIcon,
				scriptPath);
		}
	}
}
