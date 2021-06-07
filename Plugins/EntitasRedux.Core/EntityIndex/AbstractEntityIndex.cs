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
	public abstract class AbstractEntityIndex<TEntity, TKey> : IEntityIndex
		where TEntity : class, IEntity
	{
		protected readonly Func<TEntity, IComponent, TKey> _getKey;
		protected readonly Func<TEntity, IComponent, TKey[]> _getKeys;
		protected readonly IGroup<TEntity> _group;
		protected readonly bool _isSingleKey;

		protected readonly string _name;

		protected AbstractEntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey> getKey)
		{
			_name = name;
			_group = group;
			_getKey = getKey;
			_isSingleKey = true;
		}

		protected AbstractEntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey[]> getKeys)
		{
			_name = name;
			_group = group;
			_getKeys = getKeys;
			_isSingleKey = false;
		}

		public override string ToString()
		{
			return Name;
		}

		protected void IndexEntities(IGroup<TEntity> group)
		{
			foreach (var entity in group)
			{
				if (_isSingleKey)
				{
					AddEntity(_getKey(entity, null), entity);
				}
				else
				{
					var keys = _getKeys(entity, null);
					for (var i = 0; i < keys.Length; i++)
					{
						AddEntity(keys[i], entity);
					}
				}
			}
		}

		protected void OnEntityAdded(IGroup<TEntity> group, TEntity entity, int index, IComponent component)
		{
			if (_isSingleKey)
			{
				AddEntity(_getKey(entity, component), entity);
			}
			else
			{
				var keys = _getKeys(entity, component);
				for (var i = 0; i < keys.Length; i++)
				{
					AddEntity(keys[i], entity);
				}
			}
		}

		protected void OnEntityRemoved(IGroup<TEntity> group, TEntity entity, int index, IComponent component)
		{
			if (_isSingleKey)
			{
				RemoveEntity(_getKey(entity, component), entity);
			}
			else
			{
				var keys = _getKeys(entity, component);
				for (var i = 0; i < keys.Length; i++)
				{
					RemoveEntity(keys[i], entity);
				}
			}
		}

		protected abstract void AddEntity(TKey key, TEntity entity);

		protected abstract void RemoveEntity(TKey key, TEntity entity);

		protected abstract void Clear();

		~AbstractEntityIndex()
		{
			Deactivate();
		}

		public string Name => _name;

		public virtual void Activate()
		{
			_group.OnEntityAdded += OnEntityAdded;
			_group.OnEntityRemoved += OnEntityRemoved;
		}

		public virtual void Deactivate()
		{
			_group.OnEntityAdded -= OnEntityAdded;
			_group.OnEntityRemoved -= OnEntityRemoved;
			Clear();
		}
	}
}
