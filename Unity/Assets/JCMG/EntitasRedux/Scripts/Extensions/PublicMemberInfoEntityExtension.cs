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
	public static class PublicMemberInfoEntityExtension
	{
		/// <summary>
		/// Adds copies of all specified components to the target entity.
		/// If replaceExisting is true it will replace existing components
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="replaceExisting"></param>
		/// <param name="indices"></param>
		[Obsolete(
			"The CopyTo extension method for publicMemberInfo is not currently implemented and requires " +
			"refactoring")]
		public static void CopyTo(this IEntity entity, IEntity target, bool replaceExisting = false, params int[] indices)
		{
			throw new NotImplementedException(
				"The CopyTo extension method for publicMemberInfo is not currently " +
				"implemented and requires refactoring");

			/*
			var componentIndices = indices.Length == 0
				? entity.GetComponentIndices()
				: indices;
			for (var i = 0; i < componentIndices.Length; i++)
			{
				var index = componentIndices[i];
				var component = entity.GetComponent(index);
				var clonedComponent = target.CreateComponent(index, component.GetType());
				component.CopyPublicMemberValues(clonedComponent);

				if (replaceExisting)
				{
					target.ReplaceComponent(index, clonedComponent);
				}
				else
				{
					target.AddComponent(index, clonedComponent);
				}
			}
			*/
		}
	}
}
