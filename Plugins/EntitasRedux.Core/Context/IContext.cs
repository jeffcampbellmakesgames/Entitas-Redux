﻿/*

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
	public interface IContext
	{
		int TotalComponents { get; }

		Stack<IComponent>[] ComponentPools { get; }
		ContextInfo ContextInfo { get; }

		int Count { get; }
		int ReusableEntitiesCount { get; }
		int RetainedEntitiesCount { get; }

		event ContextEntityChanged OnEntityCreated;
		event ContextEntityChanged OnEntityWillBeDestroyed;
		event ContextEntityChanged OnEntityDestroyed;

		event ContextGroupChanged OnGroupCreated;

		void DestroyAllEntities();

		void AddEntityIndex(IEntityIndex entityIndex);
		IEntityIndex GetEntityIndex(string name);

		/// <summary>
		/// Returns a collection of all <see cref="IEntityIndex"/>es managed by this context,
		/// in no particular order.
		/// </summary>
		IReadOnlyCollection<IEntityIndex> EntityIndices { get; }
		void ResetCreationIndex();
		void ClearComponentPool(int index);
		void ClearComponentPools();
		void RemoveAllEventHandlers();
		void Reset();
	}

	public interface IContext<TEntity> : IContext
		where TEntity : class, IEntity
	{
		TEntity CreateEntity();

		bool HasEntity(TEntity entity);
		TEntity[] GetEntities();

		IGroup<TEntity> GetGroup(IMatcher<TEntity> matcher);
	}
}
