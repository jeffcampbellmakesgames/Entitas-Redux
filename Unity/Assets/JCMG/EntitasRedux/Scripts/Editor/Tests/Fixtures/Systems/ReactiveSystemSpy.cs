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
using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	public interface IReactiveSystemSpy {
		int didInitialize { get; }
		int didExecute { get; }
		int didCleanup { get; }
		int didTearDown { get; }
		IEntity[] entities { get; }
	}

	public class ReactiveSystemSpy : ReactiveSystem<MyTestEntity>, IReactiveSystemSpy, IInitializeSystem, ICleanupSystem, ITearDownSystem {

		public int didInitialize { get { return _didInitialize; } }
		public int didExecute { get { return _didExecute; } }
		public int didCleanup { get { return _didCleanup; } }
		public int didTearDown { get { return _didTearDown; } }
		public IEntity[] entities { get { return _entities; } }

		public Action<List<MyTestEntity>> executeAction;

		protected int _didInitialize;
		protected int _didExecute;
		protected int _didCleanup;
		protected int _didTearDown;
		protected IEntity[] _entities;

		readonly Func<MyTestEntity, bool> _filter;

		public ReactiveSystemSpy(ICollector<MyTestEntity> collector) : base(collector) {
		}

		public ReactiveSystemSpy(ICollector<MyTestEntity> collector, Func<IEntity, bool> filter) : this(collector) {
			_filter = filter;
		}

		protected override ICollector<MyTestEntity> GetTrigger(IContext<MyTestEntity> context) {
			return null;
		}

		protected override bool Filter(MyTestEntity entity) {
			return _filter == null || _filter(entity);
		}

		public void Initialize() {
			_didInitialize += 1;
		}

		protected override void Execute(List<MyTestEntity> entities) {
			_didExecute += 1;

			if (entities != null) {
				_entities = entities.ToArray();
			} else {
				_entities = null;
			}

			if (executeAction != null) {
				executeAction(entities);
			}
		}

		public void Cleanup() {
			_didCleanup += 1;
		}

		public void TearDown() {
			_didTearDown += 1;
		}
	}
}
