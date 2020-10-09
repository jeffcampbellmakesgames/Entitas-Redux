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
