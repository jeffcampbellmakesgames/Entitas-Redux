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
	internal sealed class DescribeCollector
	{
		private IContext<MyTestEntity> _context;
		private IMatcher<MyTestEntity> _matcherA;
		private IGroup<MyTestEntity> _groupA;
		private IGroup<MyTestEntity> _groupB;
		private ICollector<MyTestEntity> _collectorA;

		[SetUp]
		public void Setup()
		{
			_context = new MyTestContext();
			_matcherA = Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA);
			_groupA = _context.GetGroup(_matcherA);
			_groupB = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentB));
		}

		#region Collector Group Added

		[NUnit.Framework.Test]
		public void MatchingEntityIsCollectedWhenAddedToGroup()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var entities = _collectorA.CollectedEntities;

			// Collector should be empty
			Assert.AreEqual(0, entities.Count);

			var e = CreateEntityWithAComponent();

			// Entity should now be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));
		}

		[NUnit.Framework.Test]
		public void NonMatchingEntityIsNotCollectedWhenAddedToGroup()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var entities = _collectorA.CollectedEntities;

			// Collector should be empty
			Assert.AreEqual(0, entities.Count);

			var e = CreateEntityWithBComponent();

			// Entity should now be present.
			Assert.AreEqual(0, entities.Count);
			Assert.IsFalse(entities.Contains(e));
		}

		[NUnit.Framework.Test]
		public void MatchingEntityIsCollectedOnlyOnceWhenAddedToGroup()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var entities = _collectorA.CollectedEntities;

			// Collector should be empty
			Assert.AreEqual(0, entities.Count);

			var e = CreateEntityWithAComponent();

			// Entity should now be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));

			// Remove and Add Matching Component. Entity Should only be present once
			e.RemoveComponentA();
			e.AddComponentA();

			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));
		}

		[NUnit.Framework.Test]
		public void CollectorContentsCanBeCleared()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var entities = _collectorA.CollectedEntities;

			// Collector should be empty
			Assert.AreEqual(0, entities.Count);

			var e = CreateEntityWithAComponent();

			// Entity should now be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));

			// Collector should be empty after clearing
			_collectorA.ClearCollectedEntities();

			Assert.AreEqual(0, entities.Count);
		}

		[NUnit.Framework.Test]
		public void CollectorContentsAreClearedWhenDeactivated()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var entities = _collectorA.CollectedEntities;

			// Collector should be empty
			Assert.AreEqual(0, entities.Count);

			var e = CreateEntityWithAComponent();

			// Entity should now be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));

			// Collector should be empty after clearing
			_collectorA.Deactivate();

			Assert.AreEqual(0, entities.Count);
		}

		[NUnit.Framework.Test]
		public void CollectorWhenDeactivatedDoesNotCollectEntities()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			_collectorA.Deactivate();

			var e = CreateEntityWithAComponent();

			// Entity should NOT be present.
			var entities = _collectorA.CollectedEntities;
			Assert.AreEqual(0, entities.Count);
		}

		[NUnit.Framework.Test]
		public void CollectorCollectsEntitiesWhenActivated()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			_collectorA.Deactivate();
			_collectorA.Activate();

			var e = CreateEntityWithAComponent();

			// Entity should be present.
			var entities = _collectorA.CollectedEntities;
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));
		}

		[NUnit.Framework.Test]
		public void CollectorContinuesCollectsEntitiesWhenActivated()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var entities = _collectorA.CollectedEntities;

			_collectorA.Deactivate();

			var e1 = CreateEntityWithAComponent();

			Assert.AreEqual(0, entities.Count);

			_collectorA.Activate();

			var e2 = CreateEntityWithAComponent();

			// Entity should be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e2));
		}

		[NUnit.Framework.Test]
		public void CollectorToString()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			Assert.AreEqual("Collector(Group(AllOf(3)))", _collectorA.ToString());
		}

		[NUnit.Framework.Test]
		public void CollectorReferenceCountIsRetained()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var e = CreateEntityWithAComponent();

			var didExecute = 0;
			e.OnEntityReleased += delegate { didExecute += 1; };
			e.Destroy();

			Assert.AreEqual(1, e.RetainCount);

			if (e.AERC is SafeAERC safeAerc)
			{
				Assert.IsTrue(safeAerc.Owners.Contains(_collectorA));
			}

			Assert.AreEqual(0, didExecute);
		}

		[NUnit.Framework.Test]
		public void CollectorReferenceCountIsClearedWhenDeactivated()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var e = CreateEntityWithAComponent();

			e.Destroy();
			_collectorA.ClearCollectedEntities();

			Assert.AreEqual(0, e.RetainCount);
		}

		[NUnit.Framework.Test]
		public void CollectorReferenceCountRetainsEntitiesOnlyOnce()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Added);

			var e = CreateEntityWithAComponent();

			e.ReplaceComponentA(new ComponentA());
			e.Destroy();

			Assert.AreEqual(1, e.RetainCount);
		}

		#endregion

		#region Collector Group Removed

		[NUnit.Framework.Test]
		public void CollectorContainsEntitiesWhenRemoved()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.Removed);
			var e = CreateEntityWithAComponent();

			Assert.IsEmpty(_collectorA.CollectedEntities);

			e.RemoveComponentA();

			var entities = _collectorA.CollectedEntities;

			// Entity should be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));
		}

		#endregion

		#region Collector Group Added Or Removed

		[NUnit.Framework.Test]
		public void CollectorContainsEntitiesWhenAddedOrRemoved()
		{
			_collectorA = new Collector<MyTestEntity>(_groupA, GroupEvent.AddedOrRemoved);

			var e = CreateEntityWithAComponent();
			var entities = _collectorA.CollectedEntities;

			// Entity should be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));

			_collectorA.ClearCollectedEntities();

			e.RemoveComponentA();

			// Entity should be present.
			Assert.AreEqual(1, entities.Count);
			Assert.IsTrue(entities.Contains(e));
		}

		#endregion


		#region Collector Observing Multiple Groups

		[NUnit.Framework.Test]
		public void CollectorThrowsExceptionOnGroupCountNotEqualToGroupEventCount()
		{
			Assert.Throws<CollectorException>(
				() =>
				{
					_collectorA = new Collector<MyTestEntity>(
						new[]
						{
							_groupA
						},
						new[]
						{
							GroupEvent.Added,
							GroupEvent.Added
						});
				});
		}

		[NUnit.Framework.Test]
		public void CollectorContainsEntitiesFromTwoGroupsWhenAdded()
		{
			_collectorA = new Collector<MyTestEntity>(
			   new [] { _groupA, _groupB },
			   new [] {
				   GroupEvent.Added,
				   GroupEvent.Added
			   }
		   );

			var eA = CreateEntityWithAComponent();
			var eB = CreateEntityWithBComponent();

			Assert.AreEqual(2, _collectorA.CollectedEntities.Count);
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eA));
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eB));
		}

		[NUnit.Framework.Test]
		public void CollectorMultipleGroupsToString()
		{
			_collectorA = new Collector<MyTestEntity>(
				new[]
				{
					_groupA,
					_groupB
				},
				new[]
				{
					GroupEvent.Added,
					GroupEvent.Added
				});

			Assert.AreEqual("Collector(Group(AllOf(3)), Group(AllOf(4)))", _collectorA.ToString());
		}

		[NUnit.Framework.Test]
		public void CollectorContainsEntitiesFromTwoGroupsWhenRemoved()
		{
			_collectorA = new Collector<MyTestEntity>(
				new[]
				{
					_groupA,
					_groupB
				},
				new[]
				{
					GroupEvent.Removed,
					GroupEvent.Removed
				});

			var eA = CreateEntityWithAComponent();
			var eB = CreateEntityWithBComponent();

			Assert.AreEqual(0, _collectorA.CollectedEntities.Count);

			eA.RemoveComponentA();
			eB.RemoveComponentB();

			Assert.AreEqual(2, _collectorA.CollectedEntities.Count);
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eA));
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eB));
		}

		[NUnit.Framework.Test]
		public void CollectorContainsEntitiesFromTwoGroupsWhenAddedOrRemoved()
		{
			_collectorA = new Collector<MyTestEntity>(
				new[]
				{
					_groupA,
					_groupB
				},
				new[]
				{
					GroupEvent.AddedOrRemoved,
					GroupEvent.AddedOrRemoved
				});

			var eA = CreateEntityWithAComponent();
			var eB = CreateEntityWithBComponent();

			Assert.AreEqual(2, _collectorA.CollectedEntities.Count);
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eA));
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eB));

			_collectorA.ClearCollectedEntities();

			eA.RemoveComponentA();
			eB.RemoveComponentB();

			Assert.AreEqual(2, _collectorA.CollectedEntities.Count);
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eA));
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eB));
		}

		#endregion

		#region Collector Mixed Group Events

		[NUnit.Framework.Test]
		public void CollectorCanContainMixedGroupEventEntities()
		{
			_collectorA = new Collector<MyTestEntity>(
				new[]
				{
					_groupA,
					_groupB
				},
				new[]
				{
					GroupEvent.Added,
					GroupEvent.Removed
				});


			var eA = CreateEntityWithAComponent();
			var eB = CreateEntityWithBComponent();

			var entities = _collectorA.CollectedEntities;

			Assert.AreEqual(1, _collectorA.CollectedEntities.Count);
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eA));
			Assert.IsFalse(_collectorA.CollectedEntities.Contains(eB));

			_collectorA.ClearCollectedEntities();

			eA.RemoveComponentA();
			eB.RemoveComponentB();

			Assert.AreEqual(1, _collectorA.CollectedEntities.Count);
			Assert.IsFalse(_collectorA.CollectedEntities.Contains(eA));
			Assert.IsTrue(_collectorA.CollectedEntities.Contains(eB));
		}


		#endregion

		#region Helpers

		private MyTestEntity CreateEntityWithAComponent()
		{
			return _context.CreateEntity().AddComponentA();
		}

		private MyTestEntity CreateEntityWithBComponent()
		{
			return _context.CreateEntity().AddComponentB();
		}

		#endregion
	}
}
