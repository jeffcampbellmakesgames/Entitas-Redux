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
using JCMG.EntitasRedux.Editor;
using JCMG.Genesis.Editor;
using UnityEditor;
using UnityEngine;
using EditorGUILayoutTools = JCMG.Genesis.Editor.EditorGUILayoutTools;

namespace JCMG.EntitasRedux.Migration.Editor
{
	internal sealed class MigrationWindow : EditorWindow
	{
		private IMigration[] _migrations;
		private Vector2 _scrollViewPosition;
		private bool[] _showMigration;

		[MenuItem("Tools/JCMG/EntitasRedux/Migrate...", false, 1000)]
		public static void OpenMigrate()
		{
			var window = GetWindow<MigrationWindow>(true, $"EntitasRedux Migration - {VersionConstants.VERSION}");
			window.minSize = new Vector2(415f, 564);
			window.Show();
		}

		private void OnEnable()
		{
			_migrations = GetMigrations();
			_showMigration = new bool[_migrations.Length];
			_showMigration[0] = true;
		}

		private static IMigration[] GetMigrations()
		{
			return ReflectionTools.GetAllImplementingInstancesOfInterface<IMigration>()
				.OrderByDescending(instance => instance.GetType().FullName)
				.ToArray();
		}

		private void OnGUI()
		{
			_scrollViewPosition = EditorGUILayout.BeginScrollView(_scrollViewPosition);
			{
				var descriptionStyle = new GUIStyle(GUI.skin.label);
				descriptionStyle.wordWrap = true;
				for (var i = 0; i < _migrations.Length; i++)
				{
					var migration = _migrations[i];
					_showMigration[i] = EditorGUILayoutTools.DrawSectionHeaderToggle(migration.Version, _showMigration[i]);
					if (_showMigration[i])
					{
						EditorGUILayoutTools.BeginSectionContent();
						{
							EditorGUILayout.LabelField(migration.Description, descriptionStyle);
							if (GUILayout.Button("Apply migration " + migration.Version))
							{
								Migrate(migration, this);
							}
						}
						EditorGUILayoutTools.EndSectionContent();
					}
				}
			}
			EditorGUILayout.EndScrollView();
		}

		private static void Migrate(IMigration migration, MigrationWindow window)
		{
			var shouldMigrate = EditorUtility.DisplayDialog(
				"Migrate",
				"You are about to migrate your source files. " +
				"Make sure that you have committed your current project or that you have a backup of your project before you proceed.",
				"I have a backup - Migrate",
				"Cancel");

			if (shouldMigrate)
			{
				window.Close();
				EditorUtility.DisplayDialog(
					"Migrate",
					"Please select the folder, " + migration.WorkingDirectory + ".",
					"I will select the requested folder");

				var path = "Assets/";
				path = EditorUtility.OpenFolderPanel(migration.Version + ": " + migration.WorkingDirectory, path, string.Empty);
				if (!string.IsNullOrEmpty(path))
				{
					var changedFiles = migration.Migrate(path);
					Debug.Log("Applying " + migration.Version);
					foreach (var file in changedFiles)
					{
						MigrationUtils.WriteFiles(changedFiles);
						Debug.Log("Migrated " + file.fileName);
					}
				}
				else
				{
					throw new Exception("Could not complete migration! Selected path was invalid!");
				}
			}
		}
	}
}
