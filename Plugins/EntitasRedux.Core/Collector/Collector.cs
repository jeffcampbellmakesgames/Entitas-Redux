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
using System.Linq;
using System.Text;

namespace JCMG.EntitasRedux
{
	/// <summary>
	/// A Collector can observe one or more groups from the same context
	/// and collects changed entities based on the specified groupEvent.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class Collector<TEntity> : ICollector<TEntity>
		where TEntity : class, IEntity
	{
		private readonly HashSet<TEntity> _collectedEntities;
		private readonly GroupEvent[] _groupEvents;
		private readonly IGroup<TEntity>[] _groups;

		private readonly GroupChanged<TEntity> _addEntityCache;
		private StringBuilder _toStringBuilder;
		private string _toStringCache;

		/// <summary>
		/// Creates a Collector and will collect changed entities
		/// based on the specified groupEvent.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="groupEvent"></param>
		public Collector(IGroup<TEntity> group, GroupEvent groupEvent) : this(
			new[]
			{
				group
			},
			new[]
			{
				groupEvent
			})
		{
		}

		/// <summary>
		/// Creates a Collector and will collect changed entities
		/// based on the specified groupEvents.
		/// </summary>
		/// <param name="groups"></param>
		/// <param name="groupEvents"></param>
		public Collector(IGroup<TEntity>[] groups, GroupEvent[] groupEvents)
		{
			_groups = groups;
			_collectedEntities = new HashSet<TEntity>(EntityEqualityComparer<TEntity>.COMPARER);
			_groupEvents = groupEvents;

			if (groups.Length != groupEvents.Length)
			{
				throw new CollectorException(
					"Unbalanced count with groups (" +
					groups.Length +
					") and group events (" +
					groupEvents.Length +
					").",
					"Group and group events count must be equal.");
			}

			_addEntityCache = AddEntity;
			Activate();
		}

		private void AddEntity(IGroup<TEntity> group, TEntity entity, int index, IComponent component)
		{
			var added = _collectedEntities.Add(entity);
			if (added)
			{
				entity.Retain(this);
			}
		}

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				if (_toStringBuilder == null)
				{
					_toStringBuilder = new StringBuilder();
				}

				_toStringBuilder.Length = 0;
				_toStringBuilder.Append("Collector(");

				const string separator = ", ";
				var lastSeparator = _groups.Length - 1;
				for (var i = 0; i < _groups.Length; i++)
				{
					_toStringBuilder.Append(_groups[i]);
					if (i < lastSeparator)
					{
						_toStringBuilder.Append(separator);
					}
				}

				_toStringBuilder.Append(")");
				_toStringCache = _toStringBuilder.ToString();
			}

			return _toStringCache;
		}

		~Collector()
		{
			Deactivate();
		}

		/// <summary>
		/// Returns all collected entities.
		/// Call collector.ClearCollectedEntities()
		/// once you processed all entities.
		/// </summary>
		public HashSet<TEntity> CollectedEntities => _collectedEntities;

		/// Returns the number of all collected entities.
		public int Count => _collectedEntities.Count;

		/// <summary>
		/// Activates the Collector and will start collecting
		/// changed entities. Collectors are activated by default.
		/// </summary>
		public void Activate()
		{
			for (var i = 0; i < _groups.Length; i++)
			{
				var group = _groups[i];
				var groupEvent = _groupEvents[i];
				switch (groupEvent)
				{
					case GroupEvent.Added:
						group.OnEntityAdded -= _addEntityCache;
						group.OnEntityAdded += _addEntityCache;
						break;
					case GroupEvent.Removed:
						group.OnEntityRemoved -= _addEntityCache;
						group.OnEntityRemoved += _addEntityCache;
						break;
					case GroupEvent.AddedOrRemoved:
						group.OnEntityAdded -= _addEntityCache;
						group.OnEntityAdded += _addEntityCache;
						group.OnEntityRemoved -= _addEntityCache;
						group.OnEntityRemoved += _addEntityCache;
						break;
				}
			}
		}

		/// <summary>
		/// Deactivates the Collector.
		/// This will also clear all collected entities.
		/// Collectors are activated by default.
		/// </summary>
		public void Deactivate()
		{
			for (var i = 0; i < _groups.Length; i++)
			{
				var group = _groups[i];
				group.OnEntityAdded -= _addEntityCache;
				group.OnEntityRemoved -= _addEntityCache;
			}

			ClearCollectedEntities();
		}

		/// <summary>
		/// Returns all collected entities and casts them.
		/// Call collector.ClearCollectedEntities()
		/// once you processed all entities.
		/// </summary>
		/// <typeparam name="TCast"></typeparam>
		/// <returns></returns>
		public IEnumerable<TCast> GetCollectedEntities<TCast>()
			where TCast : class, IEntity
		{
			return _collectedEntities.Cast<TCast>();
		}

		/// <summary>
		/// Clears all collected entities.
		/// </summary>
		public void ClearCollectedEntities()
		{
			foreach (var entity in _collectedEntities)
			{
				entity.Release(this);
			}

			_collectedEntities.Clear();
		}
	}
}
