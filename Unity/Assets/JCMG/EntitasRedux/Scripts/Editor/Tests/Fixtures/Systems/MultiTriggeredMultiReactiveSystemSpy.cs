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
	public class MultiTriggeredMultiReactiveSystemSpy : MultiReactiveSystem<IMyEntity, Contexts> {

		public int DidExecute { get { return _didExecute; } }
		public IEntity[] Entities { get { return _entities; } }

		public Action<List<IMyEntity>> executeAction;

		protected int _didExecute;
		protected IEntity[] _entities;

		public MultiTriggeredMultiReactiveSystemSpy(Contexts contexts) : base(contexts) {
		}

		protected override ICollector[] GetTrigger(Contexts contexts) {
			return new ICollector[] {
				contexts.Test.CreateCollector(TestMatcher.NameAge),
				contexts.Test.CreateCollector(TestMatcher.NameAge.Removed())
			};
		}

		protected override bool Filter(IMyEntity entity) {
			return true;
		}

		protected override void Execute(List<IMyEntity> entities) {
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
	}
}
