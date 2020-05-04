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
using JCMG.Genesis.Editor;
using UnityEditor;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// User preferences for EntitasRedux
	/// </summary>
	internal static class EntitasReduxPreferences
	{
		// Menu Item
		private const string PREFERENCES_TITLE_PATH = "Preferences/EntitasRedux";

		// UI
		private static readonly IAbstractUserPreferencesDrawer[] PREFERENCES_DRAWERS;

		// Searchable Fields
		private static readonly string[] KEYWORDS;

		static EntitasReduxPreferences()
		{
			KEYWORDS = new[]
			{
				"Entitas",
				"Redux",
				"Entitas Redux",
				"EntitasRedux",
				"ECS",
				"Entity"
			};

			PREFERENCES_DRAWERS =
				ReflectionTools.GetAllImplementingInstancesOfInterface<IAbstractUserPreferencesDrawer>()
					.OrderBy(x => x.Order)
					.ToArray();

			foreach (var drawer in PREFERENCES_DRAWERS)
			{
				drawer.Initialize();
			}
		}

		[SettingsProvider]
		private static SettingsProvider CreatePersonalPreferenceSettingsProvider()
		{
			return new SettingsProvider(PREFERENCES_TITLE_PATH, SettingsScope.User)
			{
				guiHandler = DrawPersonalPrefsGUI, keywords = KEYWORDS
			};
		}

		public static void DrawGUI()
		{
			DrawPersonalPrefsGUI();
		}

		private static void DrawPersonalPrefsGUI(string value = "")
		{
			foreach (var drawer in PREFERENCES_DRAWERS)
			{
				drawer.DrawContent();
			}
		}
	}
}
