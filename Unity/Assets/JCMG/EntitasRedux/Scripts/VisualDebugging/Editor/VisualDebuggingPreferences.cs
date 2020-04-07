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

using JCMG.EntitasRedux.Editor;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	/// <summary>
	/// User preferences for visual debugging
	/// </summary>
	public static class VisualDebuggingPreferences
	{
		private static int? _systemWarningThreshold;

		private const int DEFAULT_VALUE = 5;
		private const string SYSTEM_WARNING_THRESHOLD_PREF_KEY = "EntitasRedux.VisualDebugging.SystemWarningThreshold";

		/// <summary>
		/// The threshold of time that system performance must meet or exceed to warrant appearing as a warning.
		/// </summary>
		public static int SystemWarningThreshold
		{
			get
			{
				if (!_systemWarningThreshold.HasValue)
				{
					_systemWarningThreshold = PreferenceTools.GetIntPref(SYSTEM_WARNING_THRESHOLD_PREF_KEY, DEFAULT_VALUE);
				}

				return Mathf.Max(0, _systemWarningThreshold.Value);
			}
			set
			{
				_systemWarningThreshold = Mathf.Max(0, value);
				EditorPrefs.SetInt(SYSTEM_WARNING_THRESHOLD_PREF_KEY, _systemWarningThreshold.Value);
			}
		}
	}
}
