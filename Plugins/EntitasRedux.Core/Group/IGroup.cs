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
	public interface IGroup
	{
		int Count { get; }

		void RemoveAllEventHandlers();
	}

	public interface IGroup<TEntity> : IGroup
		where TEntity : class, IEntity
	{
		IMatcher<TEntity> Matcher { get; }

		event GroupChanged<TEntity> OnEntityAdded;
		event GroupChanged<TEntity> OnEntityRemoved;
		event GroupUpdated<TEntity> OnEntityUpdated;

		void HandleEntitySilently(TEntity entity);
		void HandleEntity(TEntity entity, int index, IComponent component);
		GroupChanged<TEntity> HandleEntity(TEntity entity);

		void UpdateEntity(TEntity entity, int index, IComponent previousComponent, IComponent newComponent);

		bool ContainsEntity(TEntity entity);

		TEntity[] GetEntities();
		List<TEntity> GetEntities(List<TEntity> buffer);
		TEntity GetSingleEntity();

		IEnumerable<TEntity> AsEnumerable();
		HashSet<TEntity>.Enumerator GetEnumerator();
	}
}
