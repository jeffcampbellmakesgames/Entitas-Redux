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

using System.Collections.Generic;

namespace JCMG.EntitasRedux
{
	/// <summary>
	/// Automatic Entity Reference Counting (AERC)
	/// is used internally to prevent pooling retained entities.
	/// If you use retain manually you also have to
	/// release it manually at some point.
	/// SafeAERC checks if the entity has already been
	/// retained or released. It's slower, but you keep the information
	/// about the owners.
	/// </summary>
	public sealed class SafeAERC : IAERC
	{
		public HashSet<object> Owners => _owners;

		private readonly IEntity _entity;
		private readonly HashSet<object> _owners = new HashSet<object>();

		public SafeAERC(IEntity entity)
		{
			_entity = entity;
		}

		public int RetainCount => _owners.Count;

		public void Retain(object owner)
		{
			if (!Owners.Add(owner))
			{
				throw new EntityIsAlreadyRetainedByOwnerException(_entity, owner);
			}
		}

		public void Release(object owner)
		{
			if (!Owners.Remove(owner))
			{
				throw new EntityIsNotRetainedByOwnerException(_entity, owner);
			}
		}
	}
}
