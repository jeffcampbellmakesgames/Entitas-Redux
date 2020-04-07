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
	public static class ContextExtension
	{
		/// <summary>
		/// Returns all entities matching the specified matcher.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="context"></param>
		/// <param name="matcher"></param>
		/// <returns></returns>
		public static TEntity[] GetEntities<TEntity>(this IContext<TEntity> context, IMatcher<TEntity> matcher)
			where TEntity : class, IEntity
		{
			return context.GetGroup(matcher).GetEntities();
		}

		/// <summary>
		/// Creates a new entity and adds copies of all
		/// specified components to it.
		/// If replaceExisting is true it will replace existing components.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="context"></param>
		/// <param name="entity"></param>
		/// <param name="replaceExisting"></param>
		/// <param name="indices"></param>
		/// <returns></returns>
		[Obsolete("This method is set to be refactored and is currently not functional.", true)]
		public static TEntity CloneEntity<TEntity>(this IContext<TEntity> context,
		                                           IEntity entity,
		                                           bool replaceExisting = false,
		                                           params int[] indices)
			where TEntity : class, IEntity
		{
			var target = context.CreateEntity();
			entity.CopyTo(target, replaceExisting, indices);
			return target;
		}
	}
}
