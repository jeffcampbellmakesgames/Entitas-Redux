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
	public partial class Matcher<TEntity> : IAllOfMatcher<TEntity>
		where TEntity : class, IEntity
	{
		public string[] ComponentNames { get; set; }

		private int[] _allOfIndices;
		private int[] _anyOfIndices;

		private int[] _indices;
		private int[] _noneOfIndices;

		private Matcher()
		{
		}

		public int[] Indices
		{
			get
			{
				if (_indices == null)
				{
					_indices = MergeIndices(_allOfIndices, _anyOfIndices, _noneOfIndices);
				}

				return _indices;
			}
		}

		public int[] AllOfIndices => _allOfIndices;

		public int[] AnyOfIndices => _anyOfIndices;

		public int[] NoneOfIndices => _noneOfIndices;

		IAnyOfMatcher<TEntity> IAllOfMatcher<TEntity>.AnyOf(params int[] indices)
		{
			_anyOfIndices = DistinctIndices(indices);
			_indices = null;
			_isHashCached = false;
			return this;
		}

		IAnyOfMatcher<TEntity> IAllOfMatcher<TEntity>.AnyOf(params IMatcher<TEntity>[] matchers)
		{
			return ((IAllOfMatcher<TEntity>)this).AnyOf(MergeIndices(matchers));
		}

		public INoneOfMatcher<TEntity> NoneOf(params int[] indices)
		{
			_noneOfIndices = DistinctIndices(indices);
			_indices = null;
			_isHashCached = false;
			return this;
		}

		public INoneOfMatcher<TEntity> NoneOf(params IMatcher<TEntity>[] matchers)
		{
			return NoneOf(MergeIndices(matchers));
		}

		public bool Matches(TEntity entity)
		{
			return (_allOfIndices == null || entity.HasComponents(_allOfIndices)) &&
				   (_anyOfIndices == null || entity.HasAnyComponent(_anyOfIndices)) &&
				   (_noneOfIndices == null || !entity.HasAnyComponent(_noneOfIndices));
		}
	}
}
