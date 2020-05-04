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

using JCMG.Genesis.Editor;
using UnityEditor;
using EditorGUILayoutTools = JCMG.EntitasRedux.Editor.EditorGUILayoutTools;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	internal sealed class VisualDebuggingConfigDrawer : AbstractSettingsDrawer
	{
		public override string Title => TITLE;

		public override int Order => 51;

		private VisualDebuggingConfig _visualDebuggingConfig;

		private const string TITLE = "EntitasRedux Visual Debugging";

		public override void Initialize(GenesisSettings settings)
		{
			_visualDebuggingConfig = settings.CreateAndConfigure<VisualDebuggingConfig>();
		}

		public override void DrawHeader(GenesisSettings settings)
		{
		}

		protected override void DrawContentBody(GenesisSettings settings)
		{
			DrawDefaultInstanceCreator();
			DrawTypeDrawerFolder();
		}

		private void DrawDefaultInstanceCreator()
		{
			EditorGUILayout.BeginHorizontal();
			{
				var path = EditorGUILayoutTools.ObjectFieldOpenFolderPanel(
					"Default Instance Creators",
					_visualDebuggingConfig.DefaultInstanceCreatorFolderPath,
					_visualDebuggingConfig.DefaultInstanceCreatorFolderPath);
				if (!string.IsNullOrEmpty(path))
				{
					_visualDebuggingConfig.DefaultInstanceCreatorFolderPath = path;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawTypeDrawerFolder()
		{
			EditorGUILayout.BeginHorizontal();
			{
				var path = EditorGUILayoutTools.ObjectFieldOpenFolderPanel(
					"Type Drawers",
					_visualDebuggingConfig.TypeDrawerFolderPath,
					_visualDebuggingConfig.TypeDrawerFolderPath);
				if (!string.IsNullOrEmpty(path))
				{
					_visualDebuggingConfig.TypeDrawerFolderPath = path;
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
