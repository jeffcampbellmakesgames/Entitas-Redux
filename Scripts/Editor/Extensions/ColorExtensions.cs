using UnityEngine;

namespace JCMG.EntitasRedux.Editor
{
	/// <summary>
	/// Extension methods for <see cref="Color"/>.
	/// </summary>
	public static class ColorExtensions
	{
		private const string HEX_COLOR_RGBA_FORMAT = "{0:X2}{1:X2}{2:X2}{3:X2}";

		/// <summary>
		/// Returns the hex value of this color.
		/// </summary>
		public static string ToHexWithAlpha(this Color c)
		{
			return string.Format(
				HEX_COLOR_RGBA_FORMAT,
				(int)(c.r * 255f),
				(int)(c.g * 255f),
				(int)(c.b * 255f),
				(int)(c.a * 255f));
		}
	}
}
