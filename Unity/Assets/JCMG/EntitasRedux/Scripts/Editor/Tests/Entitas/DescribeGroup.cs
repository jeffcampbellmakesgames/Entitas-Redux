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
using JCMG.EntitasRedux;
using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeGroup
	{
		private IGroup<MyTestEntity> _groupA;
		private MyTestEntity _entityWithAComponent1;
		private MyTestEntity _entityWithAComponent2;
		private int _didDispatch = 0;
		private IEntity[] _entitiesCache = null;
		private IEntity _entityCache = null;

		[SetUp]
		public void Setup()
		{
			_groupA = new Group<MyTestEntity>(Matcher<MyTestEntity>.AllOf(CID.ComponentA));
			_entityWithAComponent1 = TestTools.CreateEntity().AddComponentA();
			_entityWithAComponent2 = TestTools.CreateEntity().AddComponentA();
			_didDispatch = 0;
		}

		#region Initial State

		[NUnit.Framework.Test]
		public void InitialGroupDoesNotContainEntitiesWhichHaveNotBeenAdded()
		{
			Assert.IsEmpty(_groupA.GetEntities());
		}

		[NUnit.Framework.Test]
		public void InitialGroupDoesNotAddEntitiesToBufferWhichHaveNotBeenAdded()
		{
			var buffer = new List<MyTestEntity>();
			buffer.Add(TestTools.CreateEntity());

			var retBuffer = _groupA.GetEntities(buffer);

			Assert.IsEmpty(buffer);
			Assert.AreEqual(buffer, retBuffer);
		}

		[NUnit.Framework.Test]
		public void InitialGroupIsEmpty()
		{
			Assert.AreEqual(0, _groupA.Count);
		}

		[NUnit.Framework.Test]
		public void InitialGroupDoesNotContainEntity()
		{
			Assert.IsFalse(_groupA.ContainsEntity(_entityWithAComponent1));
		}

		#endregion

		#region Matching Entity

		[NUnit.Framework.Test]
		public void GroupAddsMatchingEntity()
		{
			HandleSilently(_entityWithAComponent1);

			AssertContains(_entityWithAComponent1);
		}

		[NUnit.Framework.Test]
		public void GroupFillsBufferWithEntities()
		{
			HandleSilently(_entityWithAComponent1);

			var buffer = new List<MyTestEntity>();
			_groupA.GetEntities(buffer);

			Assert.AreEqual(1, buffer.Count);
			Assert.AreEqual(_entityWithAComponent1, buffer[0]);
		}

		[NUnit.Framework.Test]
		public void GroupClearsBufferBeforeFilling()
		{
			HandleSilently(_entityWithAComponent1);

			var buffer = new List<MyTestEntity>();
			buffer.Add(TestTools.CreateEntity());
			buffer.Add(TestTools.CreateEntity());

			_groupA.GetEntities(buffer);

			Assert.AreEqual(1, buffer.Count);
			Assert.AreEqual(_entityWithAComponent1, buffer[0]);
		}

		[NUnit.Framework.Test]
		public void GroupDoesNotAddSameEntityTwice()
		{
			HandleSilently(_entityWithAComponent1);
			HandleSilently(_entityWithAComponent1);

			AssertContains(_entityWithAComponent1);
		}

		[NUnit.Framework.Test]
		public void GroupCanBeEnumerated()
		{
			HandleSilently(_entityWithAComponent1);

			var i = 0;
			IEntity e = null;
			foreach (var entity in _groupA)
			{
				i++;
				e = entity;
			}

			Assert.AreEqual(1, i);
			Assert.AreEqual(_entityWithAComponent1, e);
		}

		[NUnit.Framework.Test]
		public void GroupReturnsEnumerable()
		{
			HandleSilently(_entityWithAComponent1);

			Assert.AreEqual(_entityWithAComponent1, _groupA.AsEnumerable().Single());
		}

		[NUnit.Framework.Test]
		public void GroupRemovesEntityIfNoLongerMatching()
		{
			_entityWithAComponent1.RemoveComponentA();

			HandleSilently(_entityWithAComponent1);

			AssertContainsNot(_entityWithAComponent1);
		}

		#endregion

		#region Disabled Entities

		[NUnit.Framework.Test]
		public void GroupDoesNotAddDisabledEntity()
		{
			_entityWithAComponent1.InternalDestroy();

			HandleSilently(_entityWithAComponent1);

			AssertContainsNot(_entityWithAComponent1);
		}

		#endregion

		[NUnit.Framework.Test]
		public void GroupDoesNotAddNonMatchingEntity()
		{
			var e = TestTools.CreateEntity().AddComponentB();

			HandleSilently(e);

			AssertContainsNot(e);
		}

		[NUnit.Framework.Test]
		public void GroupReturnsNullForSingleEntityWhenEmpty()
		{
			Assert.IsNull(_groupA.GetSingleEntity());
		}

		[NUnit.Framework.Test]
		public void GroupReturnsEntityForSingleEntityWhenNotEmpty()
		{
			HandleSilently(_entityWithAComponent1);

			Assert.AreEqual(_entityWithAComponent1, _groupA.GetSingleEntity());
		}

		[NUnit.Framework.Test]
		public void GroupThrowsWhenAttemptingToGetSingleEntityAndMultipleMatchingEntitiesExist()
		{
			HandleSilently(_entityWithAComponent1);
			HandleSilently(_entityWithAComponent2);

			Assert.Throws<GroupSingleEntityException<MyTestEntity>>(() => _groupA.GetSingleEntity());
		}

		#region Events

		[NUnit.Framework.Test]
		public void OnEntityAddedInvokedWhenMatchingEntityAdded()
		{
			_groupA.OnEntityAdded += (group, entity, index, component) =>
			{
				_didDispatch++;

				Assert.AreEqual(_groupA, group);
				Assert.AreEqual(_entityWithAComponent1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, component);
			};
			_groupA.OnEntityRemoved += delegate { Assert.Fail(); };
			_groupA.OnEntityUpdated += delegate { Assert.Fail(); };

			HandleAddEntityWithAComponent(_entityWithAComponent1);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnEntityAddedNotInvokedWhenMatchingEntityAlreadyAdded()
		{
			HandleAddEntityWithAComponent(_entityWithAComponent1);

			_groupA.OnEntityAdded += delegate { Assert.Fail(); };
			_groupA.OnEntityRemoved += delegate { Assert.Fail(); };
			_groupA.OnEntityUpdated += delegate { Assert.Fail(); };

			HandleAddEntityWithAComponent(_entityWithAComponent1);
		}

		[NUnit.Framework.Test]
		public void OnEntityAddedNotInvokedForNonMatchingEntity()
		{
			var e = TestTools.CreateEntity().AddComponentB();

			_groupA.OnEntityAdded += delegate { Assert.Fail(); };
			_groupA.OnEntityRemoved += delegate { Assert.Fail(); };
			_groupA.OnEntityUpdated += delegate { Assert.Fail(); };

			HandleAddEntityWithBComponent(e);
		}

		[NUnit.Framework.Test]
		public void OnEntityRemovedInvokedWhenEntityRemoved()
		{
			HandleSilently(_entityWithAComponent1);
			_groupA.OnEntityRemoved += (group, entity, index, component) =>
			{
				_didDispatch++;

				Assert.AreEqual(_groupA, group);
				Assert.AreEqual(_entityWithAComponent1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, component);
			};
			_groupA.OnEntityAdded += delegate { Assert.Fail(); };
			_groupA.OnEntityUpdated += delegate { Assert.Fail(); };

			_entityWithAComponent1.RemoveComponentA();
			HandleRemoveEntityWithAComponent(_entityWithAComponent1, Component.A);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnEntityRemovedNotInvokedWhenEntityIsNotRemoved()
		{
			_groupA.OnEntityRemoved += delegate { Assert.Fail(); };

			_entityWithAComponent1.RemoveComponentA();

			HandleRemoveEntityWithAComponent(_entityWithAComponent1, Component.A);
		}

		[NUnit.Framework.Test]
		public void MultipleDelegateInvokedWhenGroupsUpdates()
		{
			HandleSilently(_entityWithAComponent1);

			var removed = 0;
			var added = 0;
			var updated = 0;
			var newComponentA = new ComponentA();

			_groupA.OnEntityRemoved += (group, entity, index, component) =>
			{
				removed += 1;
				Assert.AreEqual(_groupA, group);
				Assert.AreEqual(_entityWithAComponent1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, component);
			};
			_groupA.OnEntityAdded += (group, entity, index, component) =>
			{
				added += 1;

				Assert.AreEqual(_groupA, group);
				Assert.AreEqual(_entityWithAComponent1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(newComponentA, component);
			};
			_groupA.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) =>
			{
				updated += 1;

				Assert.AreEqual(_groupA, group);
				Assert.AreEqual(_entityWithAComponent1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, previousComponent);
				Assert.AreEqual(newComponentA, newComponent);
			};

			HandleUpdateEntityWithAComponent(_entityWithAComponent1, newComponentA);

			Assert.AreEqual(1, removed);
			Assert.AreEqual(1, added);
			Assert.AreEqual(1, updated);
		}

		[NUnit.Framework.Test]
		public void OnEntityRemovedAndOnEntityAddedNotInvokedWhenGroupDoesNotContainEntity()
		{
			_groupA.OnEntityRemoved += delegate { Assert.Fail(); };
			_groupA.OnEntityAdded += delegate { Assert.Fail(); };
			_groupA.OnEntityUpdated += delegate { Assert.Fail(); };

			HandleUpdateEntityWithAComponent(_entityWithAComponent1, new ComponentA());
		}

		[NUnit.Framework.Test]
		public void AllEventHandlersAreRemovedFromGroup()
		{
			_groupA.OnEntityAdded += delegate { Assert.Fail(); };
			_groupA.OnEntityRemoved += delegate { Assert.Fail(); };
			_groupA.OnEntityUpdated += delegate { Assert.Fail(); };

			_groupA.RemoveAllEventHandlers();

			HandleAddEntityWithAComponent(_entityWithAComponent1);

			var cA = _entityWithAComponent1.GetComponentA();
			_entityWithAComponent1.RemoveComponentA();
			HandleRemoveEntityWithAComponent(_entityWithAComponent1, cA);

			_entityWithAComponent1.AddComponentA();
			HandleAddEntityWithAComponent(_entityWithAComponent1);
			HandleUpdateEntityWithAComponent(_entityWithAComponent1, Component.A);
		}

		#endregion

		#region Internal Caching

		[NUnit.Framework.Test]
		public void CanGetCachedEntities()
		{
			SetupCachedEntities();

			Assert.AreEqual(_entitiesCache, _groupA.GetEntities());
		}

		[NUnit.Framework.Test]
		public void CacheInvalidatedWhenAddingNewMatchingEntity()
		{
			SetupCachedEntities();

			HandleSilently(_entityWithAComponent2);

			Assert.AreNotEqual(_entitiesCache, _groupA.GetEntities());
		}

		[NUnit.Framework.Test]
		public void CacheNotUpdatedWhenAttemptingToAddNonMatchingEntity()
		{
			SetupCachedEntities();

			var e = TestTools.CreateEntity();

			HandleSilently(e);

			Assert.AreEqual(_entitiesCache, _groupA.GetEntities());
		}

		[NUnit.Framework.Test]
		public void CacheUpdatedWhenEntityIsRemovedFromGroup()
		{
			SetupCachedEntities();

			_entityWithAComponent1.RemoveComponentA();
			HandleSilently(_entityWithAComponent1);

			Assert.AreNotEqual(_entitiesCache, _groupA.GetEntities());
		}

		[NUnit.Framework.Test]
		public void CacheNotUpdatedWhenAttemptingToRemoveEntityThatWasNotPresentInGroup()
		{
			SetupCachedEntities();

			_entityWithAComponent2.RemoveComponentA();
			HandleSilently(_entityWithAComponent2);

			Assert.AreEqual(_entitiesCache, _groupA.GetEntities());
		}

		[NUnit.Framework.Test]
		public void CacheIsNotUpdatedWhenUpdatingAnEntity()
		{
			SetupCachedEntities();

			HandleUpdateEntityWithAComponent(_entityWithAComponent1, new ComponentA());

			Assert.AreEqual(_entitiesCache, _groupA.GetEntities());
		}

		#region SingleEntity

		[NUnit.Framework.Test]
		public void SingleEntityIsCached()
		{
			SetupCachedEntity();

			Assert.AreEqual(_entityCache, _groupA.GetSingleEntity());
		}

		[NUnit.Framework.Test]
		public void SingleEntityCacheInvalidatedWhenNewEntityAdded()
		{
			SetupCachedEntity();

			_entityWithAComponent1.RemoveComponentA();
			HandleSilently(_entityWithAComponent1);
			HandleSilently(_entityWithAComponent2);

			Assert.AreNotEqual(_entityCache, _groupA.GetSingleEntity());
		}

		[NUnit.Framework.Test]
		public void SingleEntityCacheInvalidatedWhenEntityIsRemoved()
		{
			SetupCachedEntity();

			_entityWithAComponent1.RemoveComponentA();
			HandleSilently(_entityWithAComponent1);

			Assert.AreNotEqual(_entityCache, _groupA.GetSingleEntity());
		}

		[NUnit.Framework.Test]
		public void SingleEntityCacheUpdatedWhenEntityIsUpdated()
		{
			SetupCachedEntity();

			HandleUpdateEntityWithAComponent(_entityWithAComponent1, new ComponentA());

			Assert.AreEqual(_entityCache, _groupA.GetSingleEntity());
		}

		#endregion

		#endregion

		#region Reference Counting

		[NUnit.Framework.Test]
		public void GroupRetainsMatchedEntity()
		{
			Assert.AreEqual(0, _entityWithAComponent1.RetainCount);

			HandleSilently(_entityWithAComponent1);

			Assert.AreEqual(1, _entityWithAComponent1.RetainCount); // Group
		}

		[NUnit.Framework.Test]
		public void GroupReleasesMatchedEntity()
		{
			HandleSilently(_entityWithAComponent1);

			_entityWithAComponent1.RemoveComponentA();

			HandleSilently(_entityWithAComponent1);

			Assert.AreEqual(0, _entityWithAComponent1.RetainCount);
		}

		[NUnit.Framework.Test]
		public void EntitiesCacheIsInvalidatedSilently()
		{
			_entityWithAComponent1.OnEntityReleased += entity =>
			{
				_didDispatch += 1;

				Assert.AreEqual(0, _groupA.GetEntities().Length);
			};

			HandleSilently(_entityWithAComponent1);
			_groupA.GetEntities();
			_entityWithAComponent1.RemoveComponentA();
			HandleSilently(_entityWithAComponent1);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void EntitiesCacheIsInvalidated()
		{
			_entityWithAComponent1.OnEntityReleased += entity =>
			{
				_didDispatch += 1;

				Assert.AreEqual(0, _groupA.GetEntities().Length);
			};

			HandleAddEntityWithAComponent(_entityWithAComponent1);
			_groupA.GetEntities();
			_entityWithAComponent1.RemoveComponentA();
			HandleRemoveEntityWithAComponent(_entityWithAComponent1, Component.A);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void SingleEntityCacheIsInvalidatedSilently()
		{
			_entityWithAComponent1.OnEntityReleased += entity =>
			{
				_didDispatch += 1;

				Assert.IsNull(_groupA.GetSingleEntity());
			};

			HandleSilently(_entityWithAComponent1);
			_groupA.GetSingleEntity();
			_entityWithAComponent1.RemoveComponentA();
			HandleSilently(_entityWithAComponent1);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void SingleEntityCacheIsInvalidated()
		{
			_entityWithAComponent1.OnEntityReleased += entity =>
			{
				_didDispatch += 1;

				Assert.IsNull(_groupA.GetSingleEntity());
			};

			HandleAddEntityWithAComponent(_entityWithAComponent1);
			_groupA.GetSingleEntity();
			_entityWithAComponent1.RemoveComponentA();
			HandleRemoveEntityWithAComponent(_entityWithAComponent1, Component.A);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void EntityIsRetainedUntilAfterEventHandlersWereCalled()
		{
			HandleAddEntityWithAComponent(_entityWithAComponent1);
			var didDispatch = 0;
			_groupA.OnEntityRemoved += (group, entity, index, component) =>
			{
				didDispatch += 1;

				Assert.AreEqual(1, entity.RetainCount);
			};
			_entityWithAComponent1.RemoveComponentA();
			HandleRemoveEntityWithAComponent(_entityWithAComponent1, Component.A);

			Assert.AreEqual(1, didDispatch);
			Assert.AreEqual(0, _entityWithAComponent1.RetainCount);
		}

		#endregion

		[NUnit.Framework.Test]
		public void ValidateToString()
		{
			var m = Matcher<TestEntity>.AllOf(Matcher<TestEntity>.AllOf(0), Matcher<TestEntity>.AllOf(1));
			var group = new Group<TestEntity>(m);

			Assert.AreEqual("Group(AllOf(0, 1))", group.ToString());
		}

		#region Helpers

		private void SetupCachedEntities()
		{
			HandleSilently(_entityWithAComponent1);
			_entitiesCache = _groupA.GetEntities();
		}

		private void SetupCachedEntity()
		{
			HandleSilently(_entityWithAComponent1);
			_entityCache = _groupA.GetSingleEntity();
		}

		private void AssertContains(params MyTestEntity[] expectedEntities)
		{
			Assert.AreEqual(expectedEntities.Length, _groupA.Count);

			var entities = _groupA.GetEntities();

			Assert.AreEqual(expectedEntities.Length, entities.Length);

			foreach (var e in expectedEntities)
			{
				Assert.Contains(e, entities);
				Assert.IsTrue(_groupA.ContainsEntity(e));
			}
		}

		private void AssertContainsNot(MyTestEntity entity)
		{
			Assert.AreEqual(0, _groupA.Count);
			Assert.IsEmpty(_groupA.GetEntities());
			Assert.IsFalse(_groupA.ContainsEntity(entity));
		}

		private void HandleSilently(MyTestEntity entity)
		{
			_groupA.HandleEntitySilently(entity);
		}

		private void Handle(MyTestEntity entity, int index, IComponent component)
		{
			_groupA.HandleEntity(entity, index, component);
		}

		private void HandleAddEntityWithAComponent(MyTestEntity entity)
		{
			Handle(entity, CID.ComponentA, entity.GetComponentA());
		}

		private void HandleAddEntityWithBComponent(MyTestEntity entity)
		{
			Handle(entity, CID.ComponentB, entity.GetComponentB());
		}

		private void HandleRemoveEntityWithAComponent(MyTestEntity entity, IComponent component)
		{
			Handle(entity, CID.ComponentA, component);
		}

		private void HandleUpdateEntityWithAComponent(MyTestEntity entity, IComponent component)
		{
			_groupA.UpdateEntity(
				entity,
				CID.ComponentA,
				Component.A,
				component);
		}

		#endregion
	}
}
