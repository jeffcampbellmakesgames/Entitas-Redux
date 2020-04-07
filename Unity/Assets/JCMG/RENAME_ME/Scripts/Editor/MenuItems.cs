using UnityEditor;
using UnityEngine;

namespace JCMG.RENAME_ME.Editor
{
	/// <summary>
	/// Menu items for this library
	/// </summary>
	internal static class MenuItems
	{
		[MenuItem("Tools/JCMG/RENAME_ME/Submit bug or feature request")]
		internal static void OpenURLToGitHubIssuesSection()
		{
			const string GITHUB_ISSUES_URL = "https://github.com/jeffcampbellmakesgames/unity-coc/issues";

			Application.OpenURL(GITHUB_ISSUES_URL);
		}

		[MenuItem("Tools/JCMG/RENAME_ME/Donate to support development")]
		internal static void OpenURLToKoFi()
		{
			const string KOFI_URL = "https://ko-fi.com/stampyturtle";

			Application.OpenURL(KOFI_URL);
		}

		[MenuItem("Tools/JCMG/RENAME_ME/About")]
		internal static void OpenAboutModalDialog()
		{
			AboutWindow.View();
		}
	}
}
