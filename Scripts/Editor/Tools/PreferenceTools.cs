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

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Helper methods for <see cref="EditorPrefs"/>.
	/// </summary>
	public static class PreferenceTools
	{
		/// <summary>
		/// Returns the current int preference; if none exists, the default is set and returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int GetIntPref(string key, int defaultValue)
		{
			if (!EditorPrefs.HasKey(key))
			{
				EditorPrefs.SetInt(key, defaultValue);
			}

			return EditorPrefs.GetInt(key);
		}

		/// <summary>
		/// Returns the current float preference; if none exists, the default is set and returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float GetFloatPref(string key, float defaultValue)
		{
			if (!EditorPrefs.HasKey(key))
			{
				EditorPrefs.SetFloat(key, defaultValue);
			}

			return EditorPrefs.GetFloat(key);
		}

		/// <summary>
		/// Returns the current string preference; if none exists, the default is set and returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetStringPref(string key, string defaultValue)
		{
			if (!EditorPrefs.HasKey(key))
			{
				EditorPrefs.SetString(key, defaultValue);
			}

			return EditorPrefs.GetString(key);
		}

		/// <summary>
		/// Returns the current bool preference; if none exists, the default is set and returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool GetBoolPref(string key, bool defaultValue)
		{
			if (!EditorPrefs.HasKey(key))
			{
				EditorPrefs.SetBool(key, defaultValue);
			}

			return EditorPrefs.GetBool(key);
		}
	}
}
