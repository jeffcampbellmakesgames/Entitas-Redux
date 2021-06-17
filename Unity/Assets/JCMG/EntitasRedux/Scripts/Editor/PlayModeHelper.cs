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

using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// A helper class for resolving general play-mode issues.
	/// </summary>
	[InitializeOnLoad]
	public static class PlayModeHelper
	{
		// Refactor Mode
		private const string REFACTOR_MODE_TITLE = "[EntitasRedux] Refactor Mode Enabled!";
		private const string REFACTOR_MODE_MSG
			= "Please disable Refactor Mode before playing in the Editor. This can result in errors due " +
			  "to parts of EntitasRedux being compiled out.";
		private const string REFACTOR_MODE_OK_TEXT = "Disable Refactor Mode";
		private const string REFACTOR_MODE_CANCEL_TEXT = "Enter PlayMode";

		private const string REFACTOR_MODE_MSG_WARNING = "[EntitasRedux] " + REFACTOR_MODE_MSG;

		static PlayModeHelper()
		{
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange playModeState)
		{
			if (RefactorModeTools.IsRefactorModeEnabled() &&
			    playModeState == PlayModeStateChange.ExitingEditMode)
			{
				var disableRefactorMode = EditorUtility.DisplayDialog(
					title: REFACTOR_MODE_TITLE,
					message: REFACTOR_MODE_MSG,
					ok: REFACTOR_MODE_OK_TEXT,
					cancel: REFACTOR_MODE_CANCEL_TEXT);

				if (disableRefactorMode)
				{
					EditorApplication.ExitPlaymode();
					RefactorModeTools.DisableRefactorMode();
				}
				else
				{
					Debug.LogWarning(REFACTOR_MODE_MSG_WARNING);
				}
			}
		}
	}
}
