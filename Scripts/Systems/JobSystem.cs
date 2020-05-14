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
using System.Threading;

namespace JCMG.EntitasRedux
{
	/// <summary>
	/// A JobSystem calls Execute(entities) with subsets of entities
	/// and distributes the workload over the specified amount of threads.
	/// Don't use the generated methods like AddXyz() and ReplaceXyz() when
	/// writing multi-threaded code in Entitas.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public abstract class JobSystem<TEntity> : IUpdateSystem
		where TEntity : class, IEntity
	{
		private readonly IGroup<TEntity> _group;
		private readonly Job<TEntity>[] _jobs;
		private readonly int _threads;

		private int _threadsRunning;

		protected JobSystem(IGroup<TEntity> group, int threads)
		{
			_group = group;
			_threads = threads;
			_jobs = new Job<TEntity>[threads];
			for (var i = 0; i < _jobs.Length; i++)
			{
				_jobs[i] = new Job<TEntity>();
			}
		}

		protected JobSystem(IGroup<TEntity> group) : this(group, Environment.ProcessorCount)
		{
		}

		private void QueueOnThread(object state)
		{
			var job = (Job<TEntity>)state;
			try
			{
				for (var i = job.from; i < job.to; i++)
				{
					Execute(job.entities[i]);
				}
			}
			catch (Exception ex)
			{
				job.exception = ex;
			}
			finally
			{
				Interlocked.Decrement(ref _threadsRunning);
			}
		}

		protected abstract void Execute(TEntity entity);

		public virtual void Update()
		{
			_threadsRunning = _threads;
			var entities = _group.GetEntities();
			var remainder = entities.Length % _threads;
			var slice = entities.Length / _threads + (remainder == 0 ? 0 : 1);
			for (var t = 0; t < _threads; t++)
			{
				var from = t * slice;
				var to = from + slice;
				if (to > entities.Length)
				{
					to = entities.Length;
				}

				var job = _jobs[t];
				job.Set(entities, from, to);
				if (from != to)
				{
					ThreadPool.QueueUserWorkItem(QueueOnThread, _jobs[t]);
				}
				else
				{
					Interlocked.Decrement(ref _threadsRunning);
				}
			}

			while (_threadsRunning != 0)
			{
			}

			foreach (var job in _jobs)
			{
				if (job.exception != null)
				{
					throw job.exception;
				}
			}
		}
	}
}
