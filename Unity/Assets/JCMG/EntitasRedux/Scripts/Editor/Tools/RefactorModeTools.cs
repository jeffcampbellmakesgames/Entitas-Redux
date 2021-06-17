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
	/// Helper methods for refactor mode
	/// </summary>
	internal static class RefactorModeTools
	{
		private const string SCRIPTING_SYMBOL = "ENTITAS_REDUX_NO_IMPL";
		private const string ENABLED_REFACTOR_MODE_MSG = "[EntitasRedux] Enabled refactor mode...";
		private const string DISABLED_REFACTOR_MODE_MSG = "[EntitasRedux] Disabled refactor mode...";

		/// <summary>
		/// Returns true if refactor mode is enabled, otherwise false.
		/// </summary>
		public static bool IsRefactorModeEnabled()
		{
			#if ENTITAS_REDUX_NO_IMPL
			return true;
			#else
			return false;
			#endif
		}

		/// <summary>
		/// Enables refactor mode by adding a unique scripting symbol `ENTITAS_REDUX_NO_IMPL` to
		/// <see cref="PlayerSettings"/> if not already present.
		/// </summary>
		public static void EnableRefactorMode()
		{
			if (EditorApplication.isCompiling)
			{
				return;
			}

			PlayerSettingsTools.AddScriptingSymbol(SCRIPTING_SYMBOL);

			Debug.Log(ENABLED_REFACTOR_MODE_MSG);

		}

		/// <summary>
		/// Enables refactor mode by removing a unique scripting symbol `ENTITAS_REDUX_NO_IMPL` from
		/// <see cref="PlayerSettings"/> if present.
		/// </summary>
		internal static void DisableRefactorMode()
		{
			if (EditorApplication.isCompiling)
			{
				return;
			}

			PlayerSettingsTools.RemoveScriptingSymbol(SCRIPTING_SYMBOL);

			Debug.Log(DISABLED_REFACTOR_MODE_MSG);
		}
	}
}
