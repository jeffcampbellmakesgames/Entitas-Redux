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

using System.Linq;
using JCMG.EntitasRedux;
using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeContext
	{
		private IContext<MyTestEntity> _defaultContext;
		private IContext<MyTestEntity> _customContext;
		private ContextInfo _contextInfo;
		private int _didDispatch;

		private MyTestEntity eAB1;
		private MyTestEntity eAB2;
		private MyTestEntity eA;
		private IMatcher<MyTestEntity> matcherAB;

		[SetUp]
		public void Setup()
		{
			_defaultContext = new MyTestContext();
			_customContext = new MyTestContext();

			_didDispatch = 0;

			matcherAB = Matcher<MyTestEntity>.AllOf(
				new[]
				{
					CID.ComponentA,
					CID.ComponentB
				});
		}

		[NUnit.Framework.Test]
		public void ValidateThatCreationIndexIncrements()
		{
			Assert.AreEqual(0, _defaultContext.CreateEntity().CreationIndex);
			Assert.AreEqual(1, _defaultContext.CreateEntity().CreationIndex);
		}

		[NUnit.Framework.Test]
		public void ValidateThatZeroEntitiesExistOnCreation()
		{
			Assert.IsEmpty(new MyTestContext().GetEntities());
			Assert.AreEqual(0, _defaultContext.Count);
		}

		[NUnit.Framework.Test]
		public void ValidateThatContextCreatesEmptyEntity()
		{
			var e = _defaultContext.CreateEntity();

			Assert.AreEqual(typeof(MyTestEntity), e.GetType());
			Assert.AreEqual(_defaultContext.TotalComponents, e.TotalComponents);
			Assert.IsTrue(e.IsEnabled);
		}

		[NUnit.Framework.Test]
		public void ValidateThatContextInitializedWithDefaultInfo()
		{
			Assert.AreEqual("MyTest", _defaultContext.ContextInfo.name);
			Assert.AreEqual(MyTestComponentsLookup.TotalComponents, _defaultContext.ContextInfo.componentNames.Length);

			for (var i = 0; i < MyTestComponentsLookup.ComponentNames.Length; i++)
			{
				Assert.AreEqual(MyTestComponentsLookup.ComponentNames[i], _defaultContext.ContextInfo.componentNames[i]);
			}
		}

		[NUnit.Framework.Test]
		public void ValidateThatContextInitializedWithComponentPools()
		{
			Assert.IsNotNull(_defaultContext.ComponentPools);
			Assert.AreEqual(MyTestComponentsLookup.TotalComponents, _defaultContext.ComponentPools.Length);
		}

		[NUnit.Framework.Test]
		public void ValidateThatContextCreatesEntityWithComponentPools()
		{
			var e = _defaultContext.CreateEntity();

			Assert.IsNotNull(e.ComponentPools);
			Assert.AreEqual(MyTestComponentsLookup.TotalComponents, e.ComponentPools.Length);
			Assert.AreEqual(_defaultContext.ComponentPools, e.ComponentPools);
		}

		[NUnit.Framework.Test]
		public void DefaultContextToString()
		{
			Assert.AreEqual("MyTest", _defaultContext.ToString());
		}

		#region When Entity Created

		[NUnit.Framework.Test]
		public void ContextEntityCount()
		{
			_defaultContext.CreateEntity();

			Assert.AreEqual(1, _defaultContext.Count);
		}

		[NUnit.Framework.Test]
		public void ContextEntityOwnership()
		{
			var e = _defaultContext.CreateEntity();

			Assert.IsTrue(_defaultContext.HasEntity(e));
			Assert.IsFalse(_defaultContext.HasEntity(_customContext.CreateEntity()));
		}

		[NUnit.Framework.Test]
		public void ContextReturnsAllCreatedEntities()
		{
			var e1 = _defaultContext.CreateEntity();
			var e2 = _defaultContext.CreateEntity();

			var entities = _defaultContext.GetEntities();
			Assert.AreEqual(2, entities.Length);
			Assert.Contains(e1, entities);
			Assert.Contains(e2, entities);
		}

		[NUnit.Framework.Test]
		public void ContextDoesNotContainDestroyedEntity()
		{
			var e = _defaultContext.CreateEntity();

			Assert.AreEqual(1, _defaultContext.Count);

			e.Destroy();

			Assert.AreEqual(0, _defaultContext.Count);

			var entities = _defaultContext.GetEntities();

			Assert.IsEmpty(entities);
		}

		[NUnit.Framework.Test]
		public void DestroyedEntityHasZeroComponents()
		{
			var e = _defaultContext.CreateEntity();
			e.AddComponentA();

			Assert.IsNotEmpty(e.GetComponents());

			e.Destroy();

			Assert.IsEmpty(e.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ValidateThatOnEntityWillBeDestroyedInvoked()
		{
			var e = _defaultContext.CreateEntity();

			var didDestroy = 0;
			_defaultContext.OnEntityWillBeDestroyed += delegate
			{
				didDestroy += 1;
			};
			e.Destroy();
			_defaultContext.CreateEntity().Destroy();

			Assert.AreEqual(2, didDestroy);
		}

		[NUnit.Framework.Test]
		public void ValidateThatAllEntitiesAreDestroyed()
		{
			var e = _defaultContext.CreateEntity();
			e.AddComponentA();

			_defaultContext.DestroyAllEntities();

			Assert.AreEqual(0, _defaultContext.Count);
			Assert.IsFalse(_defaultContext.HasEntity(e));
			Assert.IsEmpty(_defaultContext.GetEntities());
			Assert.IsEmpty(e.GetComponents());
		}

		[NUnit.Framework.Test]
		public void DeterministicOrderWhenGettingEntities()
		{
			const int NUM_ENTITIES = 10;

			for (var i = 0; i < NUM_ENTITIES; i++)
			{
				_defaultContext.CreateEntity();
			}

			var order1 = new int[NUM_ENTITIES];
			var entities1 = _defaultContext.GetEntities();
			for (var i = 0; i < NUM_ENTITIES; i++)
			{
				order1[i] = entities1[i].CreationIndex;
			}

			_defaultContext.DestroyAllEntities();
			_defaultContext.ResetCreationIndex();

			for (var i = 0; i < NUM_ENTITIES; i++)
			{
				_defaultContext.CreateEntity();
			}

			var order2 = new int[NUM_ENTITIES];
			var entities2 = _defaultContext.GetEntities();
			for (var i = 0; i < NUM_ENTITIES; i++)
			{
				order2[i] = entities2[i].CreationIndex;
			}

			for (var i = 0; i < NUM_ENTITIES; i++)
			{
				var index1 = order1[i];
				var index2 = order2[i];

				Assert.AreEqual(index1, index2);
			}
		}

		[NUnit.Framework.Test]
		public void EntitiesCannotBeDestroyedWhenRetained()
		{
			Assert.Throws<ContextStillHasRetainedEntitiesException>(
				() =>
				{
					_defaultContext.CreateEntity().Retain(new object());
					_defaultContext.DestroyAllEntities();
				});
		}

		#endregion

		#region Internal Caching

		[NUnit.Framework.Test]
		public void EntitiesAreCached()
		{
			_defaultContext.CreateEntity();

			var entities = _defaultContext.GetEntities();

			Assert.AreEqual(entities, _defaultContext.GetEntities());
		}

		[NUnit.Framework.Test]
		public void EntityCacheUpdatedWhenNewEntityCreated()
		{
			var entities = _defaultContext.GetEntities();

			_defaultContext.CreateEntity();

			Assert.AreNotEqual(entities, _defaultContext.GetEntities());
		}

		[NUnit.Framework.Test]
		public void EntityCacheUpdatedWhenEntityDestroyed()
		{
			var e = _defaultContext.CreateEntity();

			var entities = _defaultContext.GetEntities();

			Assert.AreEqual(entities, _defaultContext.GetEntities());

			e.Destroy();

			Assert.AreNotEqual(entities, _defaultContext.GetEntities());
		}

		#endregion

		#region Events

		[NUnit.Framework.Test]
		public void ValidateThatOnEntityCreatedInvoked()
		{
			IEntity eventEntity = null;

			_defaultContext.OnEntityCreated += (c, entity) =>
			{
				_didDispatch += 1;
				eventEntity = entity;
				Assert.AreEqual(c, _defaultContext);
			};

			var e = _defaultContext.CreateEntity();

			Assert.AreEqual(1, _didDispatch);
			Assert.AreEqual(eventEntity, e);
		}

		[NUnit.Framework.Test]
		public void ValidateThatOnEntityDestroyedInvoked()
		{
			IEntity eventEntity = null;

			var e = _defaultContext.CreateEntity();

			_defaultContext.OnEntityDestroyed += (c, entity) =>
			{
				_didDispatch += 1;
				eventEntity = entity;
				Assert.AreEqual(c, _defaultContext);
			};

			e.Destroy();

			Assert.AreEqual(1, _didDispatch);
			Assert.AreEqual(eventEntity, e);
		}

		[NUnit.Framework.Test]
		public void ValidateThatOnEntityWillBeDestroyedInvokedAndStateSet()
		{
			IEntity eventEntity = null;

			var e = _defaultContext.CreateEntity();
			e.AddComponentA();

			_defaultContext.OnEntityWillBeDestroyed += (c, entity) =>
			{
				_didDispatch += 1;
				eventEntity = entity;

				Assert.AreEqual(e, entity);
				Assert.AreEqual(c, _defaultContext);
				Assert.IsTrue(entity.HasComponentA());
				Assert.IsTrue(entity.IsEnabled);
				Assert.AreEqual(0, ((IContext<MyTestEntity>)c).GetEntities().Length);
			};

			e.Destroy();

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void EntityIsReleasedWhenDestroyed()
		{
			IEntity eventEntity = null;

			var e = _defaultContext.CreateEntity();
			e.AddComponentA();

			_defaultContext.OnEntityDestroyed += (c, entity) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(1, entity.RetainCount);
				eventEntity = entity;

				var newEntity = _defaultContext.CreateEntity();
				Assert.IsNotNull(newEntity);
				Assert.AreNotEqual(newEntity, entity);
			};

			e.Destroy();

			var reusedEntity = _defaultContext.CreateEntity();
			Assert.AreEqual(e, reusedEntity);
		}

		[NUnit.Framework.Test]
		public void EntityMustBeDestroyedBeforeReleased()
		{
			var e = _defaultContext.CreateEntity();
			Assert.Throws<EntityIsNotDestroyedException>(() => e.Release(_defaultContext));
		}

		[NUnit.Framework.Test]
		public void ValidateThatOnGroupCreatedInvoked()
		{
			IGroup eventGroup = null;
			_defaultContext.OnGroupCreated += (c, g) =>
			{
				_didDispatch += 1;

				eventGroup = g;
			};

			var group = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(0));

			Assert.AreEqual(1, _didDispatch);
			Assert.AreEqual(eventGroup, group);
		}

		[NUnit.Framework.Test]
		public void ValidateThatOnGroupCreatedNotInvokedForExistingGroup()
		{
			IGroup eventGroup = null;
			var group = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(0));

			_defaultContext.OnGroupCreated += (c, g) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultContext, g);

				eventGroup = g;
			};

			var duplicateGroup = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(0));

			Assert.AreEqual(group, duplicateGroup);
			Assert.AreEqual(0, _didDispatch);
			Assert.IsNull(eventGroup);
		}

		[NUnit.Framework.Test]
		public void AllExternalDelegatesAreRemovedWhenEntityIsDestroyed()
		{
			var e = _defaultContext.CreateEntity();
			e.OnComponentAdded += delegate { Assert.Fail(); };
			e.OnComponentRemoved += delegate { Assert.Fail(); };
			e.OnComponentReplaced += delegate { Assert.Fail(); };
			e.Destroy();

			var e2 = _defaultContext.CreateEntity();

			Assert.AreEqual(e, e2);

			e2.AddComponentA();
			e2.ReplaceComponentA(Component.A);
			e2.RemoveComponentA();
		}

		[NUnit.Framework.Test]
		public void ExternalDelegatesForOnEntityReleasedAreNotRemovedWhenEntityIsDestroyed()
		{
			var e = _defaultContext.CreateEntity();
			e.OnEntityReleased += entity => _didDispatch += 1;
			e.Destroy();

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void ValidateThatExternalDelegatesAreRemovedFromOnEntityReleasedAfterBeingDispatched()
		{
			var e = _defaultContext.CreateEntity();
			e.OnEntityReleased += entity => _didDispatch += 1;
			e.Destroy();
			e.Retain(this);
			e.Release(this);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void ValidateThatExternalDelegatesAreRemovedFromOnEntityReleasedAfterBeingDispatchedWhenDelayedReleased()
		{
			var e = _defaultContext.CreateEntity();
			var didRelease = 0;
			e.OnEntityReleased += entity => didRelease += 1;
			e.Retain(this);
			e.Destroy();

			Assert.AreEqual(0, didRelease);

			e.Release(this);

			Assert.AreEqual(1, didRelease);

			e.Retain(this);
			e.Release(this);

			Assert.AreEqual(1, didRelease);
		}

		#endregion

		#region Entity Pool

		[NUnit.Framework.Test]
		public void EntityCanBeCreatedFromPool()
		{
			var e = _defaultContext.CreateEntity();

			Assert.IsNotNull(e);
			Assert.AreEqual(typeof(MyTestEntity), e.GetType());
		}

		[NUnit.Framework.Test]
		public void EntityComponentsRemovedWhenDestroyed()
		{
			var e = _defaultContext.CreateEntity();
			e.AddComponentA();

			Assert.IsTrue(e.HasComponentA());

			e.Destroy();

			Assert.IsFalse(e.HasComponentA());
		}

		[NUnit.Framework.Test]
		public void EntityIsReturnedToPoolWhenDestroyed()
		{
			var e = _defaultContext.CreateEntity();
			e.AddComponentA();
			e.Destroy();

			var entity = _defaultContext.CreateEntity();

			Assert.IsFalse(entity.HasComponentA());
			Assert.AreEqual(e, entity);
		}

		[NUnit.Framework.Test]
		public void OnlyReleasedEntitesAreReturnedToPool()
		{
			var e1 = _defaultContext.CreateEntity();
			e1.Retain(this);
			e1.Destroy();

			var e2 = _defaultContext.CreateEntity();

			Assert.AreNotEqual(e1, e2);

			e1.Release(this);
			var e3 = _defaultContext.CreateEntity();

			Assert.AreEqual(e1, e3);
		}

		[NUnit.Framework.Test]
		public void NewEntityIsReturned()
		{
			var e1 = _defaultContext.CreateEntity();
			e1.AddComponentA();
			e1.Destroy();

			_defaultContext.CreateEntity();

			var e2 = _defaultContext.CreateEntity();

			Assert.IsFalse(e2.HasComponentA());
			Assert.AreNotEqual(e1, e2);
		}

		#endregion

		#region Destroyed Entities

		[NUnit.Framework.Test]
		public void ValidateThatDestroyedObjectThrowsForCommonAPIs()
		{
			var e = _defaultContext.CreateEntity();
			e.AddComponentA();
			e.Destroy();

			Assert.Throws<EntityIsNotEnabledException>(() => e.AddComponentA());
			Assert.Throws<EntityIsNotEnabledException>(() => e.RemoveComponentA());
			Assert.Throws<EntityIsNotEnabledException>(() => e.ReplaceComponentA(new ComponentA()));
			Assert.Throws<EntityIsNotEnabledException>(() => e.ReplaceComponentA(null));
			Assert.Throws<EntityIsNotEnabledException>(() => e.Destroy());
		}

		#endregion

		#region Groups

		[NUnit.Framework.Test]
		public void ValidateThatGroupIsEmptyWhenZeroEntitiesAreCreated()
		{
			var group = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentA));
			Assert.IsNotNull(group);
			Assert.IsEmpty(group.GetEntities());
		}

		[NUnit.Framework.Test]
		public void GroupIsCached()
		{
			Assert.AreEqual(_defaultContext.GetGroup(matcherAB), _defaultContext.GetGroup(matcherAB));
		}

		[NUnit.Framework.Test]
		public void GroupGetsMatchingEntities()
		{
			SetupGroupEntities();

			var g = _defaultContext.GetGroup(matcherAB).GetEntities();
			Assert.AreEqual(2, g.Length);
			Assert.Contains(eAB1, g);
			Assert.Contains(eAB2, g);
		}

		[NUnit.Framework.Test]
		public void ValidateThatCachedGroupContainsNewlyCreatedEntity()
		{
			SetupGroupEntities();

			var g = _defaultContext.GetGroup(matcherAB);
			eA.AddComponentB();
			Assert.Contains(eA, g.GetEntities());
		}

		[NUnit.Framework.Test]
		public void ValidateThatCachedGroupDoesNotContainNonMatchingEntity()
		{
			SetupGroupEntities();

			var g = _defaultContext.GetGroup(matcherAB);
			eAB1.RemoveComponentA();

			Assert.False(g.GetEntities().Contains(eAB1));
		}

		[NUnit.Framework.Test]
		public void ValidateThatDestroyedEntityIsRemovedFromGroup()
		{
			SetupGroupEntities();

			var g = _defaultContext.GetGroup(matcherAB);
			eAB1.Destroy();

			Assert.IsFalse(g.GetEntities().Contains(eAB1));
		}

		[NUnit.Framework.Test]
		public void GroupInvokesOnEntityRemovedAndOnEntityAddedWhenReplacingComponents()
		{
			SetupGroupEntities();

			var g = _defaultContext.GetGroup(matcherAB);
			var didDispatchRemoved = 0;
			var didDispatchAdded = 0;
			var componentA = new ComponentA();

			g.OnEntityRemoved += (group, entity, index, component) =>
			{
				Assert.AreEqual(g, group);
				Assert.AreEqual(eAB1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, component);

				didDispatchRemoved++;
			};

			g.OnEntityAdded += (group, entity, index, component) =>
			{
				Assert.AreEqual(g, group);
				Assert.AreEqual(eAB1, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(componentA, component);

				didDispatchAdded++;
			};
			eAB1.ReplaceComponentA(componentA);

			Assert.AreEqual(1, didDispatchRemoved);
			Assert.AreEqual(1, didDispatchAdded);
		}

		[NUnit.Framework.Test]
		public void GroupInvokesOnEntityUpdatedWhenReplacingAComponent()
		{
			SetupGroupEntities();

			var updated = 0;

			var prevComp = eA.GetComponent(CID.ComponentA);
			var newComp = new ComponentA();
			var g = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentA));
			g.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) =>
			{
				updated += 1;

				Assert.AreEqual(g, group);
				Assert.AreEqual(eA, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(prevComp, previousComponent);
				Assert.AreEqual(newComp, newComponent);
			};

			eA.ReplaceComponent(CID.ComponentA, newComp);

			Assert.AreEqual(1, updated);
		}

		[NUnit.Framework.Test]
		public void GroupWithMatcherNoneDoesNotInvokeOnEntityAdded()
		{
			var e =_defaultContext.CreateEntity()
				.AddComponentA()
				.AddComponentB();

			var matcher = Matcher<MyTestEntity>.AllOf(CID.ComponentB).NoneOf(CID.ComponentA);
			var g = _defaultContext.GetGroup(matcher);
			g.OnEntityAdded += delegate { Assert.Fail(); };
			e.Destroy();
		}

		#endregion

		#region Event Timing

		[NUnit.Framework.Test]
		public void OnEntityAddedIsInvokedAfterAllGroupsAreUpdated()
		{
			var groupA = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentA, CID.ComponentB));
			var groupB = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentB));

			groupA.OnEntityAdded += delegate
			{
				Assert.AreEqual(1, groupB.Count);
			};

			var entity = _defaultContext.CreateEntity();
			entity.AddComponentA();
			entity.AddComponentB();
		}

		[NUnit.Framework.Test]
		public void OnEntityRemovedIsInvokedAfterAllGroupsAreUpdated()
		{
			var groupB = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentB));
			var groupAB = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentA, CID.ComponentB));

			groupB.OnEntityRemoved += delegate
			{
				Assert.AreEqual(0, groupAB.Count);
			};

			var entity = _defaultContext.CreateEntity();
			entity.AddComponentA();
			entity.AddComponentB();

			entity.RemoveComponentB();
		}

		#endregion

		#region EntityIndex

		[NUnit.Framework.Test]
		public void CannotRetrieveNonExistentIndex()
		{
			Assert.Throws<ContextEntityIndexDoesNotExistException>(() => _defaultContext.GetEntityIndex("unknown"));
		}

		[NUnit.Framework.Test]
		public void EntityIndexCanBeAdded()
		{
			const int componentIndex = 1;
			var entityIndex = new PrimaryEntityIndex<MyTestEntity, string>(
				"TestIndex",
				_defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(componentIndex)),
				(arg1, arg2) => string.Empty);

			_defaultContext.AddEntityIndex(entityIndex);

			Assert.AreEqual(entityIndex, _defaultContext.GetEntityIndex(entityIndex.Name));
		}

		[NUnit.Framework.Test]
		public void CannotAddDuplicateEntityIndex()
		{
			const int componentIndex = 1;
			var entityIndex = new PrimaryEntityIndex<MyTestEntity, string>(
				"TestIndex",
				_defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(componentIndex)),
				(arg1, arg2) => string.Empty);

			_defaultContext.AddEntityIndex(entityIndex);

			Assert.Throws<ContextEntityIndexDoesAlreadyExistException>(() => _defaultContext.AddEntityIndex(entityIndex));
		}

		#endregion

		#region Reset Event Handlers

		[NUnit.Framework.Test]
		public void CreationIndexIsReset()
		{
			_defaultContext.CreateEntity();
			_defaultContext.ResetCreationIndex();

			Assert.AreEqual(0, _defaultContext.CreateEntity().CreationIndex);
		}

		[NUnit.Framework.Test]
		public void OnEntityCreatedEventHandlerIsRemoved()
		{
			_defaultContext.OnEntityCreated += delegate
			{
				Assert.Fail();
			};

			_defaultContext.RemoveAllEventHandlers();
			_defaultContext.CreateEntity();
		}

		[NUnit.Framework.Test]
		public void OnEntityWillBeDestroyedEventHandlerIsRemoved()
		{
			_defaultContext.OnEntityWillBeDestroyed += delegate
			{
				Assert.Fail();
			};

			_defaultContext.RemoveAllEventHandlers();
			_defaultContext.CreateEntity().Destroy();
		}

		[NUnit.Framework.Test]
		public void OnEntityDestroyedEventHandlerIsRemoved()
		{
			_defaultContext.OnEntityDestroyed += delegate
			{
				Assert.Fail();
			};

			_defaultContext.RemoveAllEventHandlers();
			_defaultContext.CreateEntity().Destroy();
		}

		[NUnit.Framework.Test]
		public void OnGroupCreatedEventHandlerIsRemoved()
		{
			_defaultContext.OnGroupCreated += delegate
			{
				Assert.Fail();
			};

			_defaultContext.RemoveAllEventHandlers();
			_defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(0));
		}

		#endregion

		#region Reset Component Pools

		[NUnit.Framework.Test]
		public void ComponentPoolsCanBeCleared()
		{
			SetupComponentPools();

			Assert.AreEqual(1, _defaultContext.ComponentPools[CID.ComponentA].Count);
			Assert.AreEqual(1, _defaultContext.ComponentPools[CID.ComponentB].Count);

			_defaultContext.ClearComponentPools();

			Assert.AreEqual(0, _defaultContext.ComponentPools[CID.ComponentA].Count);
			Assert.AreEqual(0, _defaultContext.ComponentPools[CID.ComponentB].Count);
		}

		[NUnit.Framework.Test]
		public void SpecificComponentPoolCanBeCleared()
		{
			SetupComponentPools();

			_defaultContext.ClearComponentPool(CID.ComponentB);

			Assert.AreEqual(1, _defaultContext.ComponentPools[CID.ComponentA].Count);
			Assert.AreEqual(0, _defaultContext.ComponentPools[CID.ComponentB].Count);
		}

		[NUnit.Framework.Test]
		public void OnlyExistingComponentPoolCanBeCleared()
		{
			SetupComponentPools();

			_defaultContext.ClearComponentPool(CID.ComponentC);
		}

		#endregion

		#region Entitas Cache

		[NUnit.Framework.Test]
		public void NewListAllocatedFromPoolPerGroup()
		{
			var groupA = _defaultContext.GetGroup(Matcher<MyTestEntity>.AllOf(CID.ComponentA));
			var groupAB = _defaultContext.GetGroup(Matcher<MyTestEntity>.AnyOf(CID.ComponentA, CID.ComponentB));
			var groupABC = _defaultContext.GetGroup(Matcher<MyTestEntity>.AnyOf(CID.ComponentA, CID.ComponentB, CID.ComponentC));

			groupA.OnEntityAdded += (g, entity, index, component) =>
			{
				_didDispatch += 1;
				entity.RemoveComponentA();
			};

			groupAB.OnEntityAdded += (g, entity, index, component) =>
			{
				_didDispatch += 1;
			};

			groupABC.OnEntityAdded += (g, entity, index, component) =>
			{
				_didDispatch += 1;
			};

			_defaultContext.CreateEntity().AddComponentA();

			Assert.AreEqual(3, _didDispatch);
		}

		#endregion

		#region Helpers

		private void SetupGroupEntities()
		{
			eAB1 = _defaultContext.CreateEntity();
			eAB1.AddComponentA();
			eAB1.AddComponentB();

			eAB2 = _defaultContext.CreateEntity();
			eAB2.AddComponentA();
			eAB2.AddComponentB();

			eA = _defaultContext.CreateEntity();
			eA.AddComponentA();
		}

		private void SetupComponentPools()
		{
			var entity = _defaultContext.CreateEntity();
			entity.AddComponentA();
			entity.AddComponentB();
			entity.RemoveComponentA();
			entity.RemoveComponentB();
		}

		#endregion
	}
}
