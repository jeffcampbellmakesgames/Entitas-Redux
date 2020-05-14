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
