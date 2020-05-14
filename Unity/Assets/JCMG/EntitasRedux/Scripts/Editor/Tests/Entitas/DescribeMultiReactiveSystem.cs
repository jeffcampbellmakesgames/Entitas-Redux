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

using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeMultiReactiveSystem
	{
		private MultiReactiveSystemSpy _multiReactiveSystemSpy;
		private MultiTriggeredMultiReactiveSystemSpy system = null;
		private TestEntity e1 = null;
		private Test2Entity e2 = null;

		#region Triggered

		[NUnit.Framework.Test]
		public void EntitiesAreProcessedFromDifferentContexts()
		{
			SetupTriggeredSystem();

			Assert.AreEqual(2, _multiReactiveSystemSpy.Entities.Length);
			Assert.Contains(e1, _multiReactiveSystemSpy.Entities);
			Assert.Contains(e2, _multiReactiveSystemSpy.Entities);

			Assert.AreEqual(52, e1.NameAge.age);
			Assert.AreEqual(34, e2.NameAge.age);
		}

		[NUnit.Framework.Test]
		public void SystemExecutesOnce()
		{
			SetupTriggeredSystem();

			Assert.AreEqual(1, _multiReactiveSystemSpy.DidExecute);
		}

		[NUnit.Framework.Test]
		public void ValidateToString()
		{
			SetupTriggeredSystem();

			Assert.AreEqual("MultiReactiveSystem(MultiReactiveSystemSpy)", _multiReactiveSystemSpy.ToString());
		}

		#endregion

		#region Multiple Collectors Triggered From Same Entity

		[NUnit.Framework.Test]
		public void MultipleCollectorSystemExecutesOnce()
		{
			SetupMultipleCollectorTriggered();

			Assert.AreEqual(1, system.DidExecute);
		}

		[NUnit.Framework.Test]
		public void MultipleCollectorSystemMergesCollectedEntitiesAndRemovesDuplicates()
		{
			SetupMultipleCollectorTriggered();

			Assert.AreEqual(1, system.Entities.Length);
		}

		[NUnit.Framework.Test]
		public void MultipleCollectorSystemClearsMergedCollectedEntities()
		{
			SetupMultipleCollectorTriggered();

			system.Execute();

			Assert.AreEqual(1, system.DidExecute);
		}

		#endregion

		#region Helpers

		private void SetupTriggeredSystem()
		{
			var contexts = new Contexts();
			_multiReactiveSystemSpy = new MultiReactiveSystemSpy(contexts);
			_multiReactiveSystemSpy.executeAction = entities =>
			{
				foreach (var e in entities)
				{
					e.NameAge.age += 10;
				}
			};

			e1 = contexts.Test.CreateEntity();
			e1.AddNameAge("Max", 42);

			e2 = contexts.Test2.CreateEntity();
			e2.AddNameAge("Jack", 24);

			_multiReactiveSystemSpy.Execute();
		}

		public void SetupMultipleCollectorTriggered()
		{
			var contexts = new Contexts();
			system = new MultiTriggeredMultiReactiveSystemSpy(contexts);
			e1 = contexts.Test.CreateEntity();
			e1.AddNameAge("Max", 42);
			e1.RemoveNameAge();

			system.Execute();
		}

		#endregion
	}
}
