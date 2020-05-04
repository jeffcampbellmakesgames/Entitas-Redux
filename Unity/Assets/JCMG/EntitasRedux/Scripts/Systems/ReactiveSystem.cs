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
	/// A ReactiveSystem calls Execute(entities) if there were changes based on
	/// the specified Collector and will only pass in changed entities.
	/// A common use-case is to react to changes, e.g. a change of the position
	/// of an entity to update the gameObject.transform.position
	/// of the related gameObject.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public abstract class ReactiveSystem<TEntity> : IReactiveSystem
		where TEntity : class, IEntity
	{
		private readonly List<TEntity> _buffer;

		private readonly ICollector<TEntity> _collector;
		private string _toStringCache;

		protected ReactiveSystem(IContext<TEntity> context)
		{
			_collector = GetTrigger(context);
			_buffer = new List<TEntity>();
		}

		protected ReactiveSystem(ICollector<TEntity> collector)
		{
			_collector = collector;
			_buffer = new List<TEntity>();
		}

		/// <summary>
		/// Specify the collector that will trigger the ReactiveSystem.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected abstract ICollector<TEntity> GetTrigger(IContext<TEntity> context);

		/// <summary>
		/// This will exclude all entities which don't pass the filter.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected abstract bool Filter(TEntity entity);

		protected abstract void Execute(List<TEntity> entities);

		public override string ToString()
		{
			if (_toStringCache == null)
			{
				_toStringCache = "ReactiveSystem(" + GetType().Name + ")";
			}

			return _toStringCache;
		}

		~ReactiveSystem()
		{
			Deactivate();
		}

		/// <summary>
		/// Activates the ReactiveSystem and starts observing changes
		/// based on the specified Collector.
		/// ReactiveSystem are activated by default.
		/// </summary>
		public void Activate()
		{
			_collector.Activate();
		}

		/// <summary>
		/// Deactivates the ReactiveSystem.
		/// No changes will be tracked while deactivated.
		/// This will also clear the ReactiveSystem.
		/// ReactiveSystem are activated by default.
		/// </summary>
		public void Deactivate()
		{
			_collector.Deactivate();
		}

		/// <summary>
		/// Clears all accumulated changes.
		/// </summary>
		public void Clear()
		{
			_collector.ClearCollectedEntities();
		}

		/// <summary>
		/// Will call Execute(entities) with changed entities
		/// if there are any. Otherwise it will not call Execute(entities).
		/// </summary>
		public void Execute()
		{
			if (_collector.Count != 0)
			{
				foreach (var e in _collector.CollectedEntities)
				{
					if (Filter(e))
					{
						e.Retain(this);
						_buffer.Add(e);
					}
				}

				_collector.ClearCollectedEntities();

				if (_buffer.Count != 0)
				{
					try
					{
						Execute(_buffer);
					}
					finally
					{
						for (var i = 0; i < _buffer.Count; i++)
						{
							_buffer[i].Release(this);
						}

						_buffer.Clear();
					}
				}
			}
		}
	}
}
