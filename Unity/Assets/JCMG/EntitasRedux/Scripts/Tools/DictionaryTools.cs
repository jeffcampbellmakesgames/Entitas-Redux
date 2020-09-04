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
	public static class DictionaryTools
	{
		/// <summary>
		/// Returns a shallow copy of <paramref name="dictionary"/> and it's contents.
		/// </summary>
		public static Dictionary<T, TV> ShallowCopy<T, TV>(Dictionary<T, TV> dictionary)
		{
			var newDict = new Dictionary<T, TV>(dictionary);
			return newDict;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="dictionary"/> where <typeparamref name="TV"/> implements
		/// <see cref="ICloneable"/>.
		/// </summary>
		public static Dictionary<T, TV> DeepCopy<T, TV>(Dictionary<T, TV> dictionary)
			where TV : ICloneable
		{
			var newDict = new Dictionary<T, TV>();
			foreach (var kvp in dictionary)
			{
				newDict.Add(kvp.Key, kvp.Value != null ? (TV)kvp.Value.Clone() : default);
			}

			return newDict;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="dictionary"/> where it's value type is a <see cref="List{TV}"/> and
		/// <typeparamref name="TV"/> implements <see cref="ICloneable"/>.
		/// </summary>
		public static Dictionary<T, List<TV>> DeepCopyListValue<T, TV>(Dictionary<T, List<TV>> dictionary)
			where TV : ICloneable
		{
			var newDict = new Dictionary<T, List<TV>>();
			foreach (var kvp in dictionary)
			{
				var newList = ListTools.DeepCopy(kvp.Value);
				newDict.Add(kvp.Key, (List<TV>)newList);
			}

			return newDict;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="dictionary"/> where it's value type is a 1D array of
		/// <typeparamref name="TV"/> which implements <see cref="ICloneable"/>.
		/// </summary>
		public static Dictionary<T, TV[]> DeepCopyArrayValue<T, TV>(Dictionary<T, TV[]> dictionary)
			where TV : ICloneable
		{
			var newDict = new Dictionary<T, TV[]>();
			foreach (var kvp in dictionary)
			{
				var newArray = ArrayTools.DeepCopy(kvp.Value);
				newDict.Add(kvp.Key, newArray);
			}

			return newDict;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="dictionary"/> where it's value type is a 2D array of
		/// <typeparamref name="TV"/> which implements <see cref="ICloneable"/>.
		/// </summary>
		public static Dictionary<T, TV[][]> DeepCopyArrayValue<T, TV>(Dictionary<T, TV[][]> dictionary)
			where TV : ICloneable
		{
			var newDict = new Dictionary<T, TV[][]>();
			foreach (var kvp in dictionary)
			{
				var newArray = ArrayTools.DeepCopy(kvp.Value);
				newDict.Add(kvp.Key, newArray);
			}

			return newDict;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="dictionary"/> where it's value type is a 3D array of
		/// <typeparamref name="TV"/> which implements <see cref="ICloneable"/>.
		/// </summary>
		public static Dictionary<T, TV[][][]> DeepCopyArrayValue<T, TV>(Dictionary<T, TV[][][]> dictionary)
			where TV : ICloneable
		{
			var newDict = new Dictionary<T, TV[][][]>();
			foreach (var kvp in dictionary)
			{
				var newArray = ArrayTools.DeepCopy(kvp.Value);
				newDict.Add(kvp.Key, newArray);
			}

			return newDict;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="dictionary"/> where it's value type is a 4D array of
		/// <typeparamref name="TV"/> which implements <see cref="ICloneable"/>.
		/// </summary>
		public static Dictionary<T, TV[][][][]> DeepCopyArrayValue<T, TV>(Dictionary<T, TV[][][][]> dictionary)
			where TV : ICloneable
		{
			var newDict = new Dictionary<T, TV[][][][]>();
			foreach (var kvp in dictionary)
			{
				var newArray = ArrayTools.DeepCopy(kvp.Value);
				newDict.Add(kvp.Key, newArray);
			}

			return newDict;
		}
	}
}
