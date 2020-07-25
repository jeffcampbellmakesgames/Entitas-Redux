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

using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Helper methods for <see cref="ColorTools"/>.
	/// </summary>
	public static class ColorTools
	{
		/// <summary>
		/// Returns a color based on the passed <paramref name="hexValue"/> string.
		/// </summary>
		/// <param name="hexValue"></param>
		/// <returns></returns>
		public static Color FromHex(string hexValue)
		{
			var r = byte.Parse(hexValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			var g = byte.Parse(hexValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			var b = byte.Parse(hexValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			var a = 0f;

			if (hexValue.Length == 8)
			{
				a = byte.Parse(hexValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
			}

			return new Color(
				r / 255f,
				g / 255f,
				b / 255f,
				a / 255f);
		}
	}
}
