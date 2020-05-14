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
		/// <summary>
		/// The threshold of time in milliseconds that system performance must meet or exceed to warrant appearing as
		/// a warning.
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

		/// <summary>
		/// The color shown for visual debug editor UX for <see cref="IFixedUpdateSystem"/>.
		/// </summary>
		public static Color FixedUpdateColor
		{
			get
			{
				if (!_fixedUpdateColor.HasValue)
				{
					_fixedUpdateColor = PreferenceTools.GetColorPref(FIXED_UPDATE_COLOR_PREF_KEY, DEFAULT_FIXED_UPDATE_COLOR);
				}

				return _fixedUpdateColor.Value;
			}
			set
			{
				_fixedUpdateColor = value;
				EditorPrefs.SetString(FIXED_UPDATE_COLOR_PREF_KEY, value.ToHexWithAlpha());
			}
		}

		/// <summary>
		/// The color shown for visual debug editor UX for <see cref="IUpdateSystem"/>.
		/// </summary>
		public static Color UpdateColor
		{
			get
			{
				if (!_updateColor.HasValue)
				{
					_updateColor = PreferenceTools.GetColorPref(UPDATE_COLOR_PREF_KEY, DEFAULT_UPDATE_COLOR);
				}

				return _updateColor.Value;
			}
			set
			{
				_updateColor = value;
				EditorPrefs.SetString(UPDATE_COLOR_PREF_KEY, value.ToHexWithAlpha());
			}
		}

		/// <summary>
		/// The color shown for visual debug editor UX for <see cref="IUpdateSystem"/>.
		/// </summary>
		public static Color LateUpdateColor
		{
			get
			{
				if (!_lateUpdateColor.HasValue)
				{
					_lateUpdateColor = PreferenceTools.GetColorPref(LATE_UPDATE_COLOR_PREF_KEY, DEFAULT_LATE_UPDATE_COLOR);
				}

				return _lateUpdateColor.Value;
			}
			set
			{
				_lateUpdateColor = value;
				EditorPrefs.SetString(LATE_UPDATE_COLOR_PREF_KEY, value.ToHexWithAlpha());
			}
		}

		/// <summary>
		/// The color shown for visual debug editor UX for <see cref="IReactiveSystem"/>.
		/// </summary>
		public static Color ReactiveColor
		{
			get
			{
				if (!_reactiveColor.HasValue)
				{
					_reactiveColor = PreferenceTools.GetColorPref(REACTIVE_COLOR_PREF_KEY, DEFAULT_REACTIVE_COLOR);
				}

				return _reactiveColor.Value;
			}
			set
			{
				_reactiveColor = value;
				EditorPrefs.SetString(REACTIVE_COLOR_PREF_KEY, value.ToHexWithAlpha());
			}
		}

		/// <summary>
		/// The color shown for visual debug editor UX for average.
		/// </summary>
		public static Color AverageColor
		{
			get
			{
				if (!_averageColor.HasValue)
				{
					_averageColor = PreferenceTools.GetColorPref(AVERAGE_COLOR_PREF_KEY, DEFAULT_AVERAGE_COLOR);
				}

				return _averageColor.Value;
			}
			set
			{
				_lateUpdateColor = value;
				EditorPrefs.SetString(AVERAGE_COLOR_PREF_KEY, value.ToHexWithAlpha());
			}
		}

		// Cache
		private static int? _systemWarningThreshold;
		private static Color? _fixedUpdateColor;
		private static Color? _updateColor;
		private static Color? _lateUpdateColor;
		private static Color? _reactiveColor;
		private static Color? _averageColor;

		// Defaults
		private const int DEFAULT_VALUE = 5;
		private static readonly Color DEFAULT_UPDATE_COLOR = new Color(0.407f, 0f, 1f); // Purple
		private static readonly Color DEFAULT_FIXED_UPDATE_COLOR = Color.cyan;
		private static readonly Color DEFAULT_LATE_UPDATE_COLOR = Color.green;
		private static readonly Color DEFAULT_REACTIVE_COLOR = Color.white;
		private static readonly Color DEFAULT_AVERAGE_COLOR = Color.yellow;

		// Pref Keys
		private const string SYSTEM_WARNING_THRESHOLD_PREF_KEY = "EntitasRedux.VisualDebugging.SystemWarningThreshold";
		private const string FIXED_UPDATE_COLOR_PREF_KEY = "EntitasRedux.VisualDebugging.FixedUpdateColor";
		private const string UPDATE_COLOR_PREF_KEY = "EntitasRedux.VisualDebugging.UpdateColor";
		private const string LATE_UPDATE_COLOR_PREF_KEY = "EntitasRedux.VisualDebugging.LateUpdateColor";
		private const string REACTIVE_COLOR_PREF_KEY = "EntitasRedux.VisualDebugging.ReactiveColor";
		private const string AVERAGE_COLOR_PREF_KEY = "EntitasRedux.VisualDebugging.AverageColor";
	}
}
