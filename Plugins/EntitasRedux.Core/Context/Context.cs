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
	/// <summary>
	/// A context manages the lifecycle of entities and groups.
	/// You can create and destroy entities and get groups of entities.
	/// The preferred way to create a context is to use the generated methods
	/// from the code generator, e.g. var context = new GameContext();
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class Context<TEntity> : IContext<TEntity>
		where TEntity : class, IEntity
	{
		private readonly Func<IEntity, IAERC> _aercFactory;
		private readonly EntityComponentReplaced _cachedComponentReplaced;
		private readonly EntityEvent _cachedDestroyEntity;

		// Cache delegates to avoid gc allocations
		private readonly EntityComponentChanged _cachedEntityChanged;
		private readonly EntityEvent _cachedEntityReleased;

		private readonly Stack<IComponent>[] _componentPools;
		private readonly ContextInfo _contextInfo;

		private readonly HashSet<TEntity> _entities = new HashSet<TEntity>(EntityEqualityComparer<TEntity>.COMPARER);
		private readonly Func<TEntity> _entityFactory;

		private readonly Dictionary<string, IEntityIndex> _entityIndices;
		private readonly ObjectPool<List<GroupChanged<TEntity>>> _groupChangedListPool;

		private readonly Dictionary<IMatcher<TEntity>, IGroup<TEntity>> _groups =
			new Dictionary<IMatcher<TEntity>, IGroup<TEntity>>();

		private readonly List<IGroup<TEntity>>[] _groupsForIndex;
		private readonly HashSet<TEntity> _retainedEntities = new HashSet<TEntity>(EntityEqualityComparer<TEntity>.COMPARER);
		private readonly Stack<TEntity> _reusableEntities = new Stack<TEntity>();

		private readonly int _totalComponents;

		private int _creationIndex;

		private TEntity[] _entitiesCache;

		/// <summary>
		/// The preferred way to create a context is to use the generated methods
		/// from the code generator, e.g. var context = new GameContext();
		/// </summary>
		/// <param name="totalComponents"></param>
		/// <param name="entityFactory"></param>
		public Context(int totalComponents, Func<TEntity> entityFactory) : this(
			totalComponents,
			0,
			null,
			null,
			entityFactory)
		{
		}

		/// <summary>
		/// The preferred way to create a context is to use the generated methods
		/// from the code generator, e.g. var context = new GameContext();
		/// </summary>
		/// <param name="totalComponents"></param>
		/// <param name="startCreationIndex"></param>
		/// <param name="contextInfo"></param>
		/// <param name="aercFactory"></param>
		/// <param name="entityFactory"></param>
		public Context(int totalComponents, int startCreationIndex, ContextInfo contextInfo, Func<IEntity, IAERC> aercFactory,
					   Func<TEntity> entityFactory)
		{
			_totalComponents = totalComponents;
			_creationIndex = startCreationIndex;

			if (contextInfo != null)
			{
				_contextInfo = contextInfo;
				if (contextInfo.componentNames.Length != totalComponents)
				{
					throw new ContextInfoException(this, contextInfo);
				}
			}
			else
			{
				_contextInfo = CreateDefaultContextInfo();
			}

			_aercFactory = aercFactory ?? (entity => new SafeAERC(entity));
			_entityFactory = entityFactory;

			_groupsForIndex = new List<IGroup<TEntity>>[totalComponents];
			_componentPools = new Stack<IComponent>[totalComponents];
			_entityIndices = new Dictionary<string, IEntityIndex>();
			_groupChangedListPool = new ObjectPool<List<GroupChanged<TEntity>>>(
				() => new List<GroupChanged<TEntity>>(),
				list => list.Clear());

			// Cache delegates to avoid gc allocations
			_cachedEntityChanged = UpdateGroupsComponentAddedOrRemoved;
			_cachedComponentReplaced = UpdateGroupsComponentReplaced;
			_cachedEntityReleased = OnEntityReleased;
			_cachedDestroyEntity = OnDestroyEntity;
		}

		private ContextInfo CreateDefaultContextInfo()
		{
			var componentNames = new string[_totalComponents];
			const string prefix = "Index ";
			for (var i = 0; i < componentNames.Length; i++)
			{
				componentNames[i] = prefix + i;
			}

			return new ContextInfo("Unnamed Context", componentNames, null);
		}

		public override string ToString()
		{
			return _contextInfo.name;
		}

		private void UpdateGroupsComponentAddedOrRemoved(IEntity entity, int index, IComponent component)
		{
			var groups = _groupsForIndex[index];
			if (groups != null)
			{
				var events = _groupChangedListPool.Get();

				var tEntity = (TEntity)entity;

				for (var i = 0; i < groups.Count; i++)
				{
					events.Add(groups[i].HandleEntity(tEntity));
				}

				for (var i = 0; i < events.Count; i++)
				{
					var groupChangedEvent = events[i];
					groupChangedEvent?.Invoke(
						groups[i],
						tEntity,
						index,
						component);
				}

				_groupChangedListPool.Push(events);
			}
		}

		private void UpdateGroupsComponentReplaced(IEntity entity, int index, IComponent previousComponent,
												   IComponent newComponent)
		{
			var groups = _groupsForIndex[index];
			if (groups != null)
			{
				var tEntity = (TEntity)entity;

				for (var i = 0; i < groups.Count; i++)
				{
					groups[i]
						.UpdateEntity(
							tEntity,
							index,
							previousComponent,
							newComponent);
				}
			}
		}

		private void OnEntityReleased(IEntity entity)
		{
			if (entity.IsEnabled)
			{
				throw new EntityIsNotDestroyedException("Cannot release " + entity + "!");
			}

			var tEntity = (TEntity)entity;
			entity.RemoveAllOnEntityReleasedHandlers();
			_retainedEntities.Remove(tEntity);
			_reusableEntities.Push(tEntity);
		}

		private void OnDestroyEntity(IEntity entity)
		{
			var tEntity = (TEntity)entity;
			var removed = _entities.Remove(tEntity);
			if (!removed)
			{
				throw new ContextDoesNotContainEntityException(
					"'" + this + "' cannot destroy " + tEntity + "!",
					"This cannot happen!?!");
			}

			_entitiesCache = null;

			OnEntityWillBeDestroyed?.Invoke(this, tEntity);

			tEntity.InternalDestroy();

			OnEntityDestroyed?.Invoke(this, tEntity);

			if (tEntity.RetainCount == 1)
			{
				// Can be released immediately without
				// adding to _retainedEntities
				tEntity.OnEntityReleased -= _cachedEntityReleased;
				_reusableEntities.Push(tEntity);
				tEntity.Release(this);
				tEntity.RemoveAllOnEntityReleasedHandlers();
			}
			else
			{
				_retainedEntities.Add(tEntity);
				tEntity.Release(this);
			}
		}

		/// <summary>
		/// Occurs when an entity gets created.
		/// </summary>
		public event ContextEntityChanged OnEntityCreated;

		/// <summary>
		/// Occurs when an entity will be destroyed.
		/// </summary>
		public event ContextEntityChanged OnEntityWillBeDestroyed;

		/// <summary>
		/// Occurs when an entity got destroyed.
		/// </summary>
		public event ContextEntityChanged OnEntityDestroyed;

		/// <summary>
		/// Occurs when a group gets created for the first time.
		/// </summary>
		public event ContextGroupChanged OnGroupCreated;

		/// <summary>
		/// The total amount of components an entity can possibly have.
		/// This value is generated by the code generator, e.g ComponentLookup.TotalComponents.
		/// </summary>
		public int TotalComponents => _totalComponents;

		/// <summary>
		/// Returns all componentPools. componentPools is used to reuse
		/// removed components.
		/// Removed components will be pushed to the componentPool.
		/// Use entity.CreateComponent(index, type) to get a new or reusable
		/// component from the componentPool.
		/// </summary>
		public Stack<IComponent>[] ComponentPools => _componentPools;

		/// <summary>
		/// The contextInfo contains information about the context.
		/// It's used to provide better error messages.
		/// </summary>
		public ContextInfo ContextInfo => _contextInfo;

		/// <summary>
		/// Returns the number of entities in the context.
		/// </summary>
		public int Count => _entities.Count;

		/// <summary>
		/// Returns the number of entities in the internal ObjectPool
		/// for entities which can be reused.
		/// </summary>
		public int ReusableEntitiesCount => _reusableEntities.Count;

		/// <summary>
		/// Returns the number of entities that are currently retained by
		/// other objects (e.g. Group, Collector, ReactiveSystem).
		/// </summary>
		public int RetainedEntitiesCount => _retainedEntities.Count;

		/// <summary>
		/// Creates a new entity or gets a reusable entity from the
		/// internal ObjectPool for entities.
		/// </summary>
		/// <returns></returns>
		public TEntity CreateEntity()
		{
			TEntity entity;

			if (_reusableEntities.Count > 0)
			{
				entity = _reusableEntities.Pop();
				entity.Reactivate(_creationIndex++);
			}
			else
			{
				entity = _entityFactory();
				entity.Initialize(
					_creationIndex++,
					_totalComponents,
					_componentPools,
					_contextInfo,
					_aercFactory(entity));
			}

			_entities.Add(entity);
			entity.Retain(this);
			_entitiesCache = null;

			entity.OnComponentAdded += _cachedEntityChanged;
			entity.OnComponentRemoved += _cachedEntityChanged;
			entity.OnComponentReplaced += _cachedComponentReplaced;
			entity.OnEntityReleased += _cachedEntityReleased;
			entity.OnDestroyEntity += _cachedDestroyEntity;

			OnEntityCreated?.Invoke(this, entity);

			return entity;
		}

		/// <summary>
		/// Destroys all entities in the context.
		/// Throws an exception if there are still retained entities.
		/// </summary>
		public void DestroyAllEntities()
		{
			var entities = GetEntities();
			for (var i = 0; i < entities.Length; i++)
			{
				entities[i].Destroy();
			}

			_entities.Clear();

			if (_retainedEntities.Count != 0)
			{
				throw new ContextStillHasRetainedEntitiesException(this, _retainedEntities.ToArray());
			}
		}

		/// <summary>
		/// Determines whether the context has the specified entity.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool HasEntity(TEntity entity)
		{
			return _entities.Contains(entity);
		}

		/// <summary>
		/// Returns all entities which are currently in the context.
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
		/// Returns a group for the specified matcher.
		/// Calling context.GetGroup(matcher) with the same matcher will always
		/// return the same instance of the group.
		/// </summary>
		/// <param name="matcher"></param>
		/// <returns></returns>
		public IGroup<TEntity> GetGroup(IMatcher<TEntity> matcher)
		{
			IGroup<TEntity> group;
			if (!_groups.TryGetValue(matcher, out group))
			{
				group = new Group<TEntity>(matcher);
				var entities = GetEntities();
				for (var i = 0; i < entities.Length; i++)
				{
					group.HandleEntitySilently(entities[i]);
				}

				_groups.Add(matcher, group);

				for (var i = 0; i < matcher.Indices.Length; i++)
				{
					var index = matcher.Indices[i];
					if (_groupsForIndex[index] == null)
					{
						_groupsForIndex[index] = new List<IGroup<TEntity>>();
					}

					_groupsForIndex[index].Add(group);
				}

				OnGroupCreated?.Invoke(this, @group);
			}

			return group;
		}

		/// <summary>
		/// Adds the IEntityIndex for the specified name.
		/// There can only be one IEntityIndex per name.
		/// </summary>
		/// <param name="entityIndex"></param>
		public void AddEntityIndex(IEntityIndex entityIndex)
		{
			if (_entityIndices.ContainsKey(entityIndex.Name))
			{
				throw new ContextEntityIndexDoesAlreadyExistException(this, entityIndex.Name);
			}

			_entityIndices.Add(entityIndex.Name, entityIndex);
		}

		/// <summary>
		/// Gets the IEntityIndex for the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEntityIndex GetEntityIndex(string name)
		{
			IEntityIndex entityIndex;
			if (!_entityIndices.TryGetValue(name, out entityIndex))
			{
				throw new ContextEntityIndexDoesNotExistException(this, name);
			}

			return entityIndex;
		}

		/// <inheritdoc />
		public IReadOnlyCollection<IEntityIndex> EntityIndices => _entityIndices.Values;

		/// <summary>
		/// Resets the creationIndex back to 0.
		/// </summary>
		public void ResetCreationIndex()
		{
			_creationIndex = 0;
		}

		/// <summary>
		/// Clears the componentPool at the specified index.
		/// </summary>
		/// <param name="index"></param>
		public void ClearComponentPool(int index)
		{
			var componentPool = _componentPools[index];
			componentPool?.Clear();
		}

		/// <summary>
		/// Clears all componentPools.
		/// </summary>
		public void ClearComponentPools()
		{
			for (var i = 0; i < _componentPools.Length; i++)
			{
				ClearComponentPool(i);
			}
		}

		/// <summary>
		/// Resets the context (destroys all entities and
		/// resets creationIndex back to 0).
		/// </summary>
		public void Reset()
		{
			DestroyAllEntities();
			ResetCreationIndex();
		}

		/// <summary>
		/// Removes all event handlers
		/// OnEntityCreated, OnEntityWillBeDestroyed,
		/// OnEntityDestroyed and OnGroupCreated
		/// </summary>
		public void RemoveAllEventHandlers()
		{
			OnEntityCreated = null;
			OnEntityWillBeDestroyed = null;
			OnEntityDestroyed = null;
			OnGroupCreated = null;
		}
	}
}
