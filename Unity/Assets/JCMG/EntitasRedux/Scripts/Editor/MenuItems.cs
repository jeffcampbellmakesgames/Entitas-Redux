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
	/// Menu items for this library
	/// </summary>
	internal static class MenuItems
	{
		private const string SCRIPTING_SYMBOL = "ENTITAS_REDUX_NO_IMPL";
		private const string ENABLED_REFACTOR_MODE_MSG = "[EntitasRedux] Enabled refactor mode...";
		private const string DISABLED_REFACTOR_MODE_MSG = "[EntitasRedux] Disabled refactor mode...";

		#region Refactor Mode

		#if !ENTITAS_REDUX_NO_IMPL

		[MenuItem("Tools/JCMG/EntitasRedux/Enable Refactor Mode #%r")]
		internal static void DisableRefactorMode()
		{
			RefactorModeTools.DisableRefactorMode();
		}

		#else

		[MenuItem("Tools/JCMG/EntitasRedux/Disable Refactor Mode #%r")]
		internal static void EnableRefactorMode()
		{
			RefactorModeTools.EnableRefactorMode();
		}

		#endif

		#endregion

		[MenuItem("Tools/JCMG/EntitasRedux/Submit bug or feature request")]
		internal static void OpenURLToGitHubIssuesSection()
		{
			const string GITHUB_ISSUES_URL = "https://github.com/jeffcampbellmakesgames/unity-coc/issues";

			Application.OpenURL(GITHUB_ISSUES_URL);
		}

		[MenuItem("Tools/JCMG/EntitasRedux/Donate to support development")]
		internal static void OpenURLToKoFi()
		{
			const string KOFI_URL = "https://ko-fi.com/stampyturtle";

			Application.OpenURL(KOFI_URL);
		}

		[MenuItem("Tools/JCMG/EntitasRedux/About")]
		internal static void OpenAboutModalDialog()
		{
			AboutWindow.View();
		}
	}
}
