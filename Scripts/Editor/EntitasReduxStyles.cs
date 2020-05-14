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
	public static class EntitasReduxStyles
	{
		public const string GROUP_BOX_STYLE = "GroupBox";

		public static GUIStyle SectionHeader
		{
			get
			{
				if (_SECTION_HEADER == null)
				{
					_SECTION_HEADER = new GUIStyle("OL Title");
					_SECTION_HEADER.fontStyle = FontStyle.Bold;
				}

				return _SECTION_HEADER;
			}
		}

		public static GUIStyle SectionContent
		{
			get
			{
				if (_SECTION_CONTENT == null)
				{
					_SECTION_CONTENT = new GUIStyle("OL Box");
					_SECTION_CONTENT.stretchHeight = false;
				}

				return _SECTION_CONTENT;
			}
		}

		private static GUIStyle _SECTION_HEADER;
		private static GUIStyle _SECTION_CONTENT;
	}
}
