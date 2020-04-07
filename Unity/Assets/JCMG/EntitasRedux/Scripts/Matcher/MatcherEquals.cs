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

namespace JCMG.EntitasRedux
{
	public partial class Matcher<TEntity>
	{
		private int _hash;
		private bool _isHashCached;

		public override bool Equals(object obj)
		{
			if (obj == null ||
			    obj.GetType() != GetType() ||
			    obj.GetHashCode() != GetHashCode())
			{
				return false;
			}

			var matcher = (Matcher<TEntity>)obj;
			if (!EqualIndices(matcher.AllOfIndices, _allOfIndices))
			{
				return false;
			}

			if (!EqualIndices(matcher.AnyOfIndices, _anyOfIndices))
			{
				return false;
			}

			if (!EqualIndices(matcher.NoneOfIndices, _noneOfIndices))
			{
				return false;
			}

			return true;
		}

		private static bool EqualIndices(int[] i1, int[] i2)
		{
			if (i1 == null != (i2 == null))
			{
				return false;
			}

			if (i1 == null)
			{
				return true;
			}

			if (i1.Length != i2.Length)
			{
				return false;
			}

			for (var i = 0; i < i1.Length; i++)
			{
				if (i1[i] != i2[i])
				{
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			if (!_isHashCached)
			{
				var hash = GetType().GetHashCode();
				hash = ApplyHash(
					hash,
					_allOfIndices,
					3,
					53);
				hash = ApplyHash(
					hash,
					_anyOfIndices,
					307,
					367);
				hash = ApplyHash(
					hash,
					_noneOfIndices,
					647,
					683);
				_hash = hash;
				_isHashCached = true;
			}

			return _hash;
		}

		private static int ApplyHash(int hash, int[] indices, int i1, int i2)
		{
			if (indices != null)
			{
				for (var i = 0; i < indices.Length; i++)
				{
					hash ^= indices[i] * i1;
				}

				hash ^= indices.Length * i2;
			}

			return hash;
		}
	}
}
