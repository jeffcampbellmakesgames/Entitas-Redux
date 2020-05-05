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
	public class EntityIndex<TEntity, TKey> : AbstractEntityIndex<TEntity, TKey>
		where TEntity : class, IEntity
	{
		private readonly Dictionary<TKey, HashSet<TEntity>> _index;

		public EntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey> getKey) : base(name, group, getKey)
		{
			_index = new Dictionary<TKey, HashSet<TEntity>>();
			Activate();
		}

		public EntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey[]> getKeys) : base(
			name,
			group,
			getKeys)
		{
			_index = new Dictionary<TKey, HashSet<TEntity>>();
			Activate();
		}

		public EntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey> getKey,
		                   IEqualityComparer<TKey> comparer) : base(name, group, getKey)
		{
			_index = new Dictionary<TKey, HashSet<TEntity>>(comparer);
			Activate();
		}

		public EntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey[]> getKeys,
		                   IEqualityComparer<TKey> comparer) : base(name, group, getKeys)
		{
			_index = new Dictionary<TKey, HashSet<TEntity>>(comparer);
			Activate();
		}

		public override void Activate()
		{
			base.Activate();
			IndexEntities(_group);
		}

		public HashSet<TEntity> GetEntities(TKey key)
		{
			if (!_index.TryGetValue(key, out var entities))
			{
				entities = new HashSet<TEntity>(EntityEqualityComparer<TEntity>.COMPARER);
				_index.Add(key, entities);
			}

			return entities;
		}

		public override string ToString()
		{
			return "EntityIndex(" + Name + ")";
		}

		protected override void Clear()
		{
			foreach (var entities in _index.Values)
			{
				foreach (var entity in entities)
				{
					if (entity.AERC is SafeAERC safeAerc)
					{
						if (safeAerc.Owners.Contains(this))
						{
							entity.Release(this);
						}
					}
					else
					{
						entity.Release(this);
					}
				}
			}

			_index.Clear();
		}

		protected override void AddEntity(TKey key, TEntity entity)
		{
			GetEntities(key).Add(entity);

			if (entity.AERC is SafeAERC safeAerc)
			{
				if (!safeAerc.Owners.Contains(this))
				{
					entity.Retain(this);
				}
			}
			else
			{
				entity.Retain(this);
			}
		}

		protected override void RemoveEntity(TKey key, TEntity entity)
		{
			GetEntities(key).Remove(entity);

			if (entity.AERC is SafeAERC safeAerc)
			{
				if (safeAerc.Owners.Contains(this))
				{
					entity.Release(this);
				}
			}
			else
			{
				entity.Release(this);
			}
		}
	}
}
