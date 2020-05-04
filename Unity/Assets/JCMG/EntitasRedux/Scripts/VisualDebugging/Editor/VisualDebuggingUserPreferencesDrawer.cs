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
using JCMG.EntitasRedux.Editor;
using JCMG.Genesis.Editor;
using UnityEditor;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	internal sealed class VisualDebuggingUserPreferencesDrawer : AbstractUserPreferencesDrawer
	{
		private bool _enableDeviceDeepProfiling;
		private bool _enableVisualDebugging;
		private ScriptingDefineSymbols _scriptingDefineSymbols;

		// UI
		private const string TITLE = "Visual Debugging";

		// Scripting Symbols
		private const string ENTITAS_DISABLE_VISUAL_DEBUGGING = "ENTITAS_DISABLE_VISUAL_DEBUGGING";
		private const string ENTITAS_DISABLE_DEEP_PROFILING = "ENTITAS_DISABLE_DEEP_PROFILING";

		/// <summary>
		/// Performs any needed setup or initialization prior to drawing user preferences
		/// </summary>
		public override void Initialize()
		{
			_scriptingDefineSymbols = new ScriptingDefineSymbols();
			_enableVisualDebugging = !_scriptingDefineSymbols.BuildTargetToDefSymbol.Values
				.All(defs => defs.Contains(ENTITAS_DISABLE_VISUAL_DEBUGGING));
			_enableDeviceDeepProfiling = !_scriptingDefineSymbols.BuildTargetToDefSymbol.Values
				.All(defs => defs.Contains(ENTITAS_DISABLE_DEEP_PROFILING));
		}

		/// <summary>
		/// Draws user preferences
		/// </summary>
		public override void DrawContent()
		{
			using (new EditorGUILayout.VerticalScope(EntitasReduxStyles.GROUP_BOX_STYLE))
			{
				EditorGUILayout.LabelField(TITLE, EditorStyles.boldLabel);
				using (var scope = new EditorGUI.ChangeCheckScope())
				{
					_enableVisualDebugging = EditorGUILayout.Toggle("Enable Visual Debugging", _enableVisualDebugging);

					if (scope.changed)
					{
						if (_enableVisualDebugging)
						{
							_scriptingDefineSymbols.RemoveDefineSymbol(ENTITAS_DISABLE_VISUAL_DEBUGGING);
						}
						else
						{
							_scriptingDefineSymbols.AddDefineSymbol(ENTITAS_DISABLE_VISUAL_DEBUGGING);
						}
					}
				}

				using (var scope = new EditorGUI.ChangeCheckScope())
				{
					_enableDeviceDeepProfiling = EditorGUILayout.Toggle("Enable Device Profiling", _enableDeviceDeepProfiling);

					if (scope.changed)
					{
						if (_enableDeviceDeepProfiling)
						{
							_scriptingDefineSymbols.RemoveDefineSymbol(ENTITAS_DISABLE_DEEP_PROFILING);
						}
						else
						{
							_scriptingDefineSymbols.AddDefineSymbol(ENTITAS_DISABLE_DEEP_PROFILING);
						}
					}
				}

				using (new EditorGUILayout.HorizontalScope())
				{
					EditorGUILayout.LabelField("System Warning Threshold");
					using (var scope = new EditorGUI.ChangeCheckScope())
					{
						var newValue = EditorGUILayout.IntField(VisualDebuggingPreferences.SystemWarningThreshold);

						if (scope.changed)
						{
							VisualDebuggingPreferences.SystemWarningThreshold = newValue;
						}
					}
				}
			}
		}
	}
}
