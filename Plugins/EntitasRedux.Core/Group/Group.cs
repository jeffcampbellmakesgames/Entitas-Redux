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
	/// Use context.GetGroup(matcher) to get a group of entities which match
	/// the specified matcher. Calling context.GetGroup(matcher) with the
	/// same matcher will always return the same instance of the group.
	/// The created group is managed by the context and will always be up to date.
	/// It will automatically add entities that match the matcher or
	/// remove entities as soon as they don't match the matcher anymore.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class Group<TEntity> : IGroup<TEntity>
		where TEntity : class, IEntity
	{
		private readonly HashSet<TEntity> _entities;

		private readonly IMatcher<TEntity> _matcher;

		private TEntity[] _entitiesCache;
		private TEntity _singleEntityCache;
		private string _toStringCache;

		/// <summary>
		/// Use context.GetGroup(matcher) to get a group of entities which match
		/// the specified matcher.
		/// </summary>
		/// <param name="matcher"></param>
		public Group(IMatcher<TEntity> matcher)
		{
			_entities = new HashSet<TEntity>(EntityEqualityComparer<TEntity>.COMPARER);
			_matcher = matcher;
		}

		private bool AddEntitySilently(TEntity entity)
		{
			if (entity.IsEnabled)
			{
				var added = _entities.Add(entity);
				if (added)
				{
					_entitiesCache = null;
					_singleEntityCache = null;
					entity.Retain(this);
				}

				return added;
			}

			return false;
		}

		private void AddEntity(TEntity entity, int index, IComponent component)
		{
			if (AddEntitySilently(entity))
			{
				OnEntityAdded?.Invoke(
					this,
					entity,
					index,
					component);
			}
		}

		private bool RemoveEntitySilently(TEntity entity)
		{
			var removed = _entities.Remove(entity);
			if (removed)
			{
				_entitiesCache = null;
				_singleEntityCache = null;
				entity.Release(this);
			}

			return removed;
		}

		private void RemoveEntity(TEntity entity, int index, IComponent component)
		{
			var removed = _entities.Remove(entity);
			if (removed)
			{
				_entitiesCache = null;
				_singleEntityCache = null;
				OnEntityRemoved?.Invoke(
					this,
					entity,
					index,
					component);

				entity.Release(this);
			}
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				_toStringCache = "Group(" + _matcher + ")";
			}

			return _toStringCache;
		}

		/// <summary>
		/// Occurs when an entity gets added.
		/// </summary>
		public event GroupChanged<TEntity> OnEntityAdded;

		/// <summary>
		/// Occurs when an entity gets removed.
		/// </summary>
		public event GroupChanged<TEntity> OnEntityRemoved;

		/// <summary>
		/// Occurs when a component of an entity in the group gets replaced.
		/// </summary>
		public event GroupUpdated<TEntity> OnEntityUpdated;

		/// <summary>
		/// Returns the number of entities in the group.
		/// </summary>
		public int Count => _entities.Count;

		/// <summary>
		/// Returns the matcher which was used to create this group.
		/// </summary>
		public IMatcher<TEntity> Matcher => _matcher;

		/// <summary>
		/// This is used by the context to manage the group.
		/// </summary>
		/// <param name="entity"></param>
		public void HandleEntitySilently(TEntity entity)
		{
			if (_matcher.Matches(entity))
			{
				AddEntitySilently(entity);
			}
			else
			{
				RemoveEntitySilently(entity);
			}
		}

		/// <summary>
		/// This is used by the context to manage the group.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="index"></param>
		/// <param name="component"></param>
		public void HandleEntity(TEntity entity, int index, IComponent component)
		{
			if (_matcher.Matches(entity))
			{
				AddEntity(entity, index, component);
			}
			else
			{
				RemoveEntity(entity, index, component);
			}
		}

		/// <summary>
		/// This is used by the context to manage the group.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="index"></param>
		/// <param name="previousComponent"></param>
		/// <param name="newComponent"></param>
		public void UpdateEntity(TEntity entity, int index, IComponent previousComponent, IComponent newComponent)
		{
			if (_entities.Contains(entity))
			{
				OnEntityRemoved?.Invoke(
					this,
					entity,
					index,
					previousComponent);

				OnEntityAdded?.Invoke(
					this,
					entity,
					index,
					newComponent);

				OnEntityUpdated?.Invoke(
					this,
					entity,
					index,
					previousComponent,
					newComponent);
			}
		}

		/// <summary>
		/// Removes all event handlers from this group.
		/// Keep in mind that this will break reactive systems and
		/// entity indices which rely on this group.
		/// </summary>
		public void RemoveAllEventHandlers()
		{
			OnEntityAdded = null;
			OnEntityRemoved = null;
			OnEntityUpdated = null;
		}

		public GroupChanged<TEntity> HandleEntity(TEntity entity)
		{
			return _matcher.Matches(entity)
				? AddEntitySilently(entity) ? OnEntityAdded : null
				: RemoveEntitySilently(entity)
					? OnEntityRemoved
					: null;
		}

		/// <summary>
		/// Determines whether this group has the specified entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool ContainsEntity(TEntity entity)
		{
			return _entities.Contains(entity);
		}

		/// <summary>
		/// Returns all entities which are currently in this group.
		/// </summary>
		/// <returns></returns>
		public TEntity[] GetEntities()
		{
			if (_entitiesCache == null)
			{
				_entitiesCache = new TEntity[_entities.Count];
				_entities.CopyTo(_entitiesCache);
			}

			return _entitiesCache;
		}

		/// <summary>
		/// Fills the buffer with all entities which are currently in this group.
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public List<TEntity> GetEntities(List<TEntity> buffer)
		{
			buffer.Clear();
			buffer.AddRange(_entities);
			return buffer;
		}

		public IEnumerable<TEntity> AsEnumerable()
		{
			return _entities;
		}

		public HashSet<TEntity>.Enumerator GetEnumerator()
		{
			return _entities.GetEnumerator();
		}

		/// <summary>
		/// Returns the only entity in this group. It will return null
		/// if the group is empty. It will throw an exception if the group
		/// has more than one entity.
		/// </summary>
		/// <returns></returns>
		public TEntity GetSingleEntity()
		{
			if (_singleEntityCache == null)
			{
				var c = _entities.Count;
				if (c == 1)
				{
					using (var enumerator = _entities.GetEnumerator())
					{
						enumerator.MoveNext();
						_singleEntityCache = enumerator.Current;
					}
				}
				else if (c == 0)
				{
					return null;
				}
				else
				{
					throw new GroupSingleEntityException<TEntity>(this);
				}
			}

			return _singleEntityCache;
		}
	}
}
