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

using JCMG.EntitasRedux;
using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeReactiveSystem
	{
		private MyTestContext _context;
		private IContext<MyTestEntity> _context1;
		private IContext<MyTestEntity> _context2;
		private ReactiveSystemSpy _system;

		private readonly IMatcher<MyTestEntity> _matcherAb = Matcher<MyTestEntity>.AllOf(CID.ComponentA, CID.ComponentB);

		[SetUp]
		public void Setup()
		{
			_context = new MyTestContext();
		}

		#region OnEntityAdded

		[NUnit.Framework.Test]
		public void ReactiveSystemDesNotExecuteWhenZeroEntitiesCollectedOnAdded()
		{
			SetupReactiveSystemAdded();

			_system.Execute();

			AssertEntities(_system, null);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesWhenTriggeredOnAdded()
		{
			SetupReactiveSystemAdded();

			var e = CreateEntityAb();
			_system.Execute();
			AssertEntities(_system, e);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesWhenTriggeredOnlyOnceOnAdded()
		{
			SetupReactiveSystemAdded();

			var e = CreateEntityAb();
			_system.Execute();
			_system.Execute();
			AssertEntities(_system, e);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemRetainsAndReleasesCollectedEntitiesOnAdded()
		{
			SetupReactiveSystemAdded();

			var e = CreateEntityAb();
			var retainCount = e.RetainCount;
			_system.Execute();

			Assert.AreEqual(3, retainCount);   // retained by context, group and collector
			Assert.AreEqual(2, e.RetainCount); // retained by context and group
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemCollectsChangedEntitiesInExecuteOnAdded()
		{
			SetupReactiveSystemAdded();

			var e = CreateEntityAb();
			_system.executeAction = entities =>
			{
				entities[0].ReplaceComponentA(Component.A);
			};

			_system.Execute();
			_system.Execute();

			AssertEntities(_system, e, 2);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemCollectsCreatedEntitiesInExecuteOnAdded()
		{
			SetupReactiveSystemAdded();

			var e1 = CreateEntityAb();
			MyTestEntity e2 = null;
			_system.executeAction = entities =>
			{
				if (e2 == null)
				{
					e2 = CreateEntityAb();
				}
			};

			_system.Execute();
			AssertEntities(_system, e1);

			_system.Execute();
			AssertEntities(_system, e2, 2);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemDoesNotExecuteWhenNotTriggeredOnAdded()
		{
			SetupReactiveSystemAdded();

			_context.CreateEntity().AddComponentA();
			_system.Execute();
			AssertEntities(_system, null);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemDeactivatesAndWillNotTriggerOnAdded()
		{
			SetupReactiveSystemAdded();

			_system.Deactivate();
			CreateEntityAb();
			_system.Execute();
			AssertEntities(_system, null);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemActivatedWillTriggerAgainOnAdded()
		{
			SetupReactiveSystemAdded();

			_system.Deactivate();
			_system.Activate();
			var e = CreateEntityAb();
			_system.Execute();
			AssertEntities(_system, e);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemClearsOnAdded()
		{
			SetupReactiveSystemAdded();

			CreateEntityAb();
			_system.Clear();
			_system.Execute();
			AssertEntities(_system, null);
		}

		[NUnit.Framework.Test]
		public void ValidateReactiveSystemToStringOnAdded()
		{
			SetupReactiveSystemAdded();

			Assert.AreEqual("ReactiveSystem(ReactiveSystemSpy)", _system.ToString());
		}

		#endregion

		#region OnEntityRemoved

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesWhenTriggeredOnRemoved()
		{
			SetupReactiveSystemRemoved();

			var e = CreateEntityAb()
				.RemoveComponentA();

			_system.Execute();
			AssertEntities(_system, e);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesOnlyOnceWhenTriggeredOnRemoved()
		{
			SetupReactiveSystemRemoved();

			var e = CreateEntityAb()
				.RemoveComponentA();

			_system.Execute();
			_system.Execute();
			AssertEntities(_system, e);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemDoesNotExecuteWhenNotTriggeredOnRemoved()
		{
			SetupReactiveSystemRemoved();

			CreateEntityAb()
							.AddComponentC()
							.RemoveComponentC();

			_system.Execute();
			AssertEntities(_system, null);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemRetainsEntitiesUntilExecuteCompletedOnRemoved()
		{
			SetupReactiveSystemRemoved();

			var e = CreateEntityAb();
			var didExecute = 0;
			_system.executeAction = entities =>
			{
				didExecute += 1;

				Assert.AreEqual(1, entities[0].RetainCount);
			};

			e.Destroy();
			_system.Execute();

			Assert.AreEqual(1, didExecute);
			Assert.AreEqual(0, e.RetainCount);
		}

		#endregion

		#region OnEntityAddedOrRemoved

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesWhenAdded()
		{
			SetupReactiveSystemAddedOrRemoved();

			var e = CreateEntityAb();
			_system.Execute();
			AssertEntities(_system, e);
		}

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesWhenAddedOrRemoved()
		{
			SetupReactiveSystemAddedOrRemoved();

			var e = CreateEntityAb();
			_system.Execute();
			e.RemoveComponentA();
			_system.Execute();
			AssertEntities(_system, e, 2);
		}

		#endregion

		#region Multiple Contexts

		[NUnit.Framework.Test]
		public void ReactiveSystemExecutesWhenTriggeredByACollector()
		{
			SetupMultipleContexts();

			var eA1 = _context1.CreateEntity().AddComponentA();
			_context2.CreateEntity().AddComponentA();

			var eB1 = _context1.CreateEntity().AddComponentB();
			var eB2 = _context2.CreateEntity().AddComponentB();

			_system.Execute();
			AssertEntities(_system, eA1);

			eB1.RemoveComponentB();
			eB2.RemoveComponentB();
			_system.Execute();
			AssertEntities(_system, eB2, 2);
		}

		#endregion

		#region Filters Entities

		[NUnit.Framework.Test]
		public void FiltersEntities()
		{
			_system = new ReactiveSystemSpy(_context.CreateCollector(_matcherAb),
							e => ((NameAgeComponent)e.GetComponent(CID.ComponentA)).age > 42);

			_context.CreateEntity()
				.AddComponentA()
				.AddComponentC();

			var eAb1 = _context.CreateEntity();
			eAb1.AddComponentB();
			eAb1.AddComponent(CID.ComponentA, new NameAgeComponent { age = 10 });

			var eAb2 = _context.CreateEntity();
			eAb2.AddComponentB();
			eAb2.AddComponent(CID.ComponentA, new NameAgeComponent { age = 50 });

			var didExecute = 0;
			_system.executeAction = entities =>
			{
				didExecute += 1;

				Assert.AreEqual(3, eAb2.RetainCount); // retained by context, group and collector
			};

			_system.Execute();

			Assert.AreEqual(1, didExecute);

			_system.Execute();

			Assert.AreEqual(1, _system.entities.Length);
			Assert.AreEqual(eAb2, _system.entities[0]);

			Assert.AreEqual(2, eAb1.RetainCount); // retained by context and group
			Assert.AreEqual(2, eAb2.RetainCount);
		}

		#endregion

		#region Clears

		[NUnit.Framework.Test]
		public void ClearsReactiveSystemAfterExecute()
		{
			_system = new ReactiveSystemSpy(_context.CreateCollector(_matcherAb));
			_system.executeAction = entities =>
			{
				entities[0].ReplaceComponentA(Component.A);
			};

			var e = CreateEntityAb();
			_system.Execute();
			_system.Clear();
			_system.Execute();
			AssertEntities(_system, e);
		}

		#endregion

		#region Helpers

		private void SetupReactiveSystemAdded()
		{
			_system = new ReactiveSystemSpy(_context.CreateCollector(_matcherAb));
		}

		private void SetupReactiveSystemRemoved()
		{
			_system = new ReactiveSystemSpy(_context.CreateCollector(_matcherAb.Removed()));
		}

		private void SetupReactiveSystemAddedOrRemoved()
		{
			_system = new ReactiveSystemSpy(_context.CreateCollector(_matcherAb.AddedOrRemoved()));
		}

		private void SetupMultipleContexts()
		{
			_context1 = new MyTestContext();
			_context2 = new MyTestContext();

			var groupA = _context1.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentA));
			var groupB = _context2.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentB));

			var groups = new[] { groupA, groupB };
			var groupEvents = new[] {
							GroupEvent.Added,
							GroupEvent.Removed
						};
			var collector = new Collector<MyTestEntity>(groups, groupEvents);

			_system = new ReactiveSystemSpy(collector);
		}

		static void AssertEntities(IReactiveSystemSpy system, MyTestEntity entity, int didExecute = 1)
		{
			if (entity == null)
			{
				Assert.AreEqual(0, system.didExecute);
				Assert.IsNull(system.entities);

			}
			else
			{
				Assert.AreEqual(didExecute, system.didExecute);
				Assert.AreEqual(1, system.entities.Length);
				Assert.Contains(entity, system.entities);
			}
		}

		MyTestEntity CreateEntityAb()
		{
			return _context.CreateEntity()
				.AddComponentA()
				.AddComponentB();
		}

		MyTestEntity CreateEntityAc()
		{
			return _context.CreateEntity()
				.AddComponentA()
				.AddComponentC();
		}

		MyTestEntity CreateEntityAbc()
		{
			return _context.CreateEntity()
				.AddComponentA()
				.AddComponentB()
				.AddComponentC();
		}

		#endregion
	}
}
