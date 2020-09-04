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
using System.Linq;

namespace JCMG.EntitasRedux
{
	public static class CollectionExtension
	{
		/// <summary>
		/// Returns the only entity in the collection.
		/// It will throw an exception if the collection doesn't have
		/// exactly one entity.
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static IEntity SingleEntity(this ICollection<IEntity> collection)
		{
			if (collection.Count != 1)
			{
				throw new SingleEntityException(collection.Count);
			}

			return collection.First();
		}

		/// <summary>
		/// Returns the only entity in the collection.
		/// It will throw an exception if the collection doesn't have
		/// exactly one entity.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static TEntity SingleEntity<TEntity>(this ICollection<TEntity> collection)
			where TEntity : class, IEntity
		{
			if (collection.Count != 1)
			{
				throw new SingleEntityException(collection.Count);
			}

			return collection.First();
		}

		/// <summary>
		/// Performs a naive deep copy of an <see cref="IList{T}"/> where the deep-copy of each <typeparamref name="T"/>
		/// value is performed by calling <see cref="ICloneable.Clone"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static IList<T> DeepCopy<T>(this IList<T> list) where T : ICloneable
		{
			var newList = (IList<T>)Activator.CreateInstance(list.GetType());
			for (var i = 0; i < list.Count; i++)
			{
				newList.Add((T)list[i].Clone());
			}

			return newList;
		}

		/// <summary>
		/// Performs a shallow copy of an  <see cref="IList{T}"/> to a new list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static IList<T> ShallowCopy<T>(this IList<T> list)
		{
			var newList = (IList<T>)Activator.CreateInstance(list.GetType());
			for (var i = 0; i < list.Count; i++)
			{
				newList.Add(list[i]);
			}

			return newList;
		}
	}
}
