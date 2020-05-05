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

using System.Text;

namespace JCMG.EntitasRedux
{
	public partial class Matcher<TEntity>
	{
		private StringBuilder _toStringBuilder;

		private string _toStringCache;

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				if (_toStringBuilder == null)
				{
					_toStringBuilder = new StringBuilder();
				}

				_toStringBuilder.Length = 0;
				if (_allOfIndices != null)
				{
					AppendIndices(
						_toStringBuilder,
						"AllOf",
						_allOfIndices,
						ComponentNames);
				}

				if (_anyOfIndices != null)
				{
					if (_allOfIndices != null)
					{
						_toStringBuilder.Append(".");
					}

					AppendIndices(
						_toStringBuilder,
						"AnyOf",
						_anyOfIndices,
						ComponentNames);
				}

				if (_noneOfIndices != null)
				{
					AppendIndices(
						_toStringBuilder,
						".NoneOf",
						_noneOfIndices,
						ComponentNames);
				}

				_toStringCache = _toStringBuilder.ToString();
			}

			return _toStringCache;
		}

		private static void AppendIndices(StringBuilder sb, string prefix, int[] indexArray, string[] componentNames)
		{
			const string separator = ", ";
			sb.Append(prefix);
			sb.Append("(");
			var lastSeparator = indexArray.Length - 1;
			for (var i = 0; i < indexArray.Length; i++)
			{
				var index = indexArray[i];
				if (componentNames == null)
				{
					sb.Append(index);
				}
				else
				{
					sb.Append(componentNames[index]);
				}

				if (i < lastSeparator)
				{
					sb.Append(separator);
				}
			}

			sb.Append(")");
		}
	}
}
