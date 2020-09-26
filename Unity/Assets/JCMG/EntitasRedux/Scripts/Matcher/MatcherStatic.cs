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

using System;
using System.Collections.Generic;

namespace JCMG.EntitasRedux
{
	public partial class Matcher<TEntity>
	{
		/// <summary>
		/// Lazy-loaded index buffer (ThreadStatic)
		/// </summary>
		private static List<int> IndexBuffer => _indexBufferThreadStatic = _indexBufferThreadStatic ?? new List<int>();

		/// <summary>
		/// Lazy-loaded index set buffer (ThreadStatic)
		/// </summary>
		private static HashSet<int> IndexSetBuffer => _indexSetBufferThreadStatic = _indexSetBufferThreadStatic ?? new HashSet<int>();

		[ThreadStatic]
		private static List<int> _indexBufferThreadStatic = new List<int>();

		[ThreadStatic]
		private static HashSet<int> _indexSetBufferThreadStatic = new HashSet<int>();

		public static IAllOfMatcher<TEntity> AllOf(params int[] indices)
		{
			var matcher = new Matcher<TEntity>();
			matcher._allOfIndices = DistinctIndices(indices);
			return matcher;
		}

		public static IAllOfMatcher<TEntity> AllOf(params IMatcher<TEntity>[] matchers)
		{
			var allOfMatcher = (Matcher<TEntity>)AllOf(MergeIndices(matchers));
			SetComponentNames(allOfMatcher, matchers);
			return allOfMatcher;
		}

		public static IAnyOfMatcher<TEntity> AnyOf(params int[] indices)
		{
			var matcher = new Matcher<TEntity>();
			matcher._anyOfIndices = DistinctIndices(indices);
			return matcher;
		}

		public static IAnyOfMatcher<TEntity> AnyOf(params IMatcher<TEntity>[] matchers)
		{
			var anyOfMatcher = (Matcher<TEntity>)AnyOf(MergeIndices(matchers));
			SetComponentNames(anyOfMatcher, matchers);
			return anyOfMatcher;
		}

		private static int[] MergeIndices(int[] allOfIndices, int[] anyOfIndices, int[] noneOfIndices)
		{
			if (allOfIndices != null)
			{
				IndexBuffer.AddRange(allOfIndices);
			}

			if (anyOfIndices != null)
			{
				IndexBuffer.AddRange(anyOfIndices);
			}

			if (noneOfIndices != null)
			{
				IndexBuffer.AddRange(noneOfIndices);
			}

			var mergedIndices = DistinctIndices(IndexBuffer);

			IndexBuffer.Clear();

			return mergedIndices;
		}

		private static int[] MergeIndices(IMatcher<TEntity>[] matchers)
		{
			var indices = new int[matchers.Length];
			for (var i = 0; i < matchers.Length; i++)
			{
				var matcher = matchers[i];
				if (matcher.Indices.Length != 1)
				{
					throw new MatcherException(matcher.Indices.Length);
				}

				indices[i] = matcher.Indices[0];
			}

			return indices;
		}

		private static string[] GetComponentNames(IMatcher<TEntity>[] matchers)
		{
			for (var i = 0; i < matchers.Length; i++)
			{
				if (matchers[i] is Matcher<TEntity> matcher && matcher.ComponentNames != null)
				{
					return matcher.ComponentNames;
				}
			}

			return null;
		}

		private static void SetComponentNames(Matcher<TEntity> matcher, IMatcher<TEntity>[] matchers)
		{
			var componentNames = GetComponentNames(matchers);
			if (componentNames != null)
			{
				matcher.ComponentNames = componentNames;
			}
		}

		private static int[] DistinctIndices(IList<int> indices)
		{
			foreach (var index in indices)
			{
				IndexSetBuffer.Add(index);
			}

			var uniqueIndices = new int[IndexSetBuffer.Count];
			IndexSetBuffer.CopyTo(uniqueIndices);
			Array.Sort(uniqueIndices);

			IndexSetBuffer.Clear();

			return uniqueIndices;
		}
	}
}
