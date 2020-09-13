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

namespace JCMG.EntitasRedux
{
	/// <summary>
	/// Helper methods for arrays
	/// </summary>
	public static class ArrayTools
	{
		/// <summary>
		/// Returns a deep copy of <paramref name="array"/> where <typeparamref name="T"/> implements
		/// <see cref="ICloneable"/>.
		/// </summary>
		public static T[] DeepCopy<T>(T[] array)
			where T : ICloneable
		{
			var newArray = new T[array.Length];
			for (var i = 0; i < array.Length; i++)
			{
				newArray[i] = array[i] != null ? (T)array[i].Clone() : default;
			}

			return newArray;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="array"/> where <typeparamref name="T"/> implements
		/// <see cref="ICloneable"/>.
		/// </summary>
		public static T[][] DeepCopy<T>(T[][] array)
			where T : ICloneable
		{
			var newArray = (T[][])array.Clone();
			for (var i = 0; i < array.Length; i++)
			{
				for (var j = 0; j < array[i].Length; j++)
				{
					newArray[i][j] = array[i][j] != null ? (T)array[i][j].Clone() : default;
				}
			}

			return newArray;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="array"/> where <typeparamref name="T"/> implements
		/// <see cref="ICloneable"/>.
		/// </summary>
		public static T[][][] DeepCopy<T>(T[][][] array)
			where T : ICloneable
		{
			var newArray = (T[][][])array.Clone();
			for (var i = 0; i < array.Length; i++)
			{
				for (var j = 0; j < array[i].Length; j++)
				{
					for (var k = 0; k < array[i][k].Length; k++)
					{
						newArray[i][j][k] = array[i][j][k] != null ? (T)array[i][j][k].Clone() : default;
					}
				}
			}

			return newArray;
		}

		/// <summary>
		/// Returns a deep copy of <paramref name="array"/> where <typeparamref name="T"/> implements
		/// <see cref="ICloneable"/>.
		/// </summary>
		public static T[][][][] DeepCopy<T>(T[][][][] array)
			where T : ICloneable
		{
			var newArray = (T[][][][])array.Clone();
			for (var i = 0; i < array.Length; i++)
			{
				for (var j = 0; j < array[i].Length; j++)
				{
					for (var k = 0; k < array[i][k].Length; k++)
					{
						for (var l = 0; l < array[i][j][k].Length; l++)
						{
							newArray[i][j][k][l] = array[i][j][k][l] != null ? (T)array[i][j][k][l].Clone() : default;
						}
					}
				}
			}

			return newArray;
		}
	}
}
