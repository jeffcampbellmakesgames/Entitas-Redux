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
