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
	internal class DescribeEntity
	{
		private readonly int[] _indicesA = { CID.ComponentA };
		private readonly int[] _indicesAB = { CID.ComponentA, CID.ComponentB };

		private MyTestContext _context;
		private MyTestEntity _defaultEntity;
		private int _didDispatch;
		private IComponent[] _componentCache;
		private int[] _componentIndicesCache;
		private string _description;

		[SetUp]
		public void Setup()
		{
			_context = new MyTestContext();
			_defaultEntity = TestTools.CreateEntity();
			_didDispatch = 0;
		}

		#region Default Context Info

		[NUnit.Framework.Test]
		public void EntityHasDefaultContextInfo()
		{
			Assert.AreEqual("No Context", _defaultEntity.ContextInfo.name);
			Assert.AreEqual(CID.TotalComponents, _defaultEntity.ContextInfo.componentNames.Length);
			Assert.IsNull(_defaultEntity.ContextInfo.componentTypes);

			for (var i = 0; i < _defaultEntity.ContextInfo.componentNames.Length; i++)
			{
				Assert.AreEqual(i.ToString(), _defaultEntity.ContextInfo.componentNames[i]);
			}
		}

		#endregion

		#region Initial State

		[NUnit.Framework.Test]
		public void ValidateEntityInitialization()
		{
			var contextInfo = new ContextInfo(null, null, null);
			var componentPools = new Stack<IComponent>[42];
			var e = new TestEntity();
			e.Initialize(1, 2, componentPools, contextInfo);

			Assert.IsTrue(e.IsEnabled);
			Assert.AreEqual(1, e.CreationIndex);
			Assert.AreEqual(2, e.TotalComponents);
			Assert.AreEqual(componentPools, e.ComponentPools);
			Assert.AreEqual(contextInfo, e.ContextInfo);
		}

		[NUnit.Framework.Test]
		public void EntityReactivatesAfterBeingDestroyed()
		{
			var contextInfo = new ContextInfo(null, null, null);
			var componentPools = new Stack<IComponent>[42];
			var e = new TestEntity();
			e.Initialize(1, 2, componentPools, contextInfo);
			e.InternalDestroy();
			e.Reactivate(42);

			Assert.IsTrue(e.IsEnabled);
			Assert.AreEqual(42, e.CreationIndex);
			Assert.AreEqual(2, e.TotalComponents);
			Assert.AreEqual(componentPools, e.ComponentPools);
			Assert.AreEqual(contextInfo, e.ContextInfo);
		}

		[NUnit.Framework.Test]
		public void CannotRetrieveNonExistentComponentFromEntity()
		{
			Assert.Throws<EntityDoesNotHaveComponentException>(() => _defaultEntity.GetComponentA());
		}

		[NUnit.Framework.Test]
		public void ValidateTotalComponentCount()
		{
			Assert.AreEqual(CID.TotalComponents, _defaultEntity.TotalComponents);
		}

		[NUnit.Framework.Test]
		public void ValidateInitialComponentAreEmpty()
		{
			Assert.IsEmpty(_defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ValidateInitialComponentIndicesAreEmpty()
		{
			Assert.IsEmpty(_defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ValidateUninitializedComponentNotPresent()
		{
			Assert.IsFalse(_defaultEntity.HasComponentA());
		}

		[NUnit.Framework.Test]
		public void ValidateUninitializedComponentByIndexNotPresentA()
		{
			Assert.IsFalse(_defaultEntity.HasComponents(_indicesA));
		}

		[NUnit.Framework.Test]
		public void ValidateUninitializedComponentByIndexNotPresentB()
		{
			Assert.IsFalse(_defaultEntity.HasAnyComponent(_indicesA));
		}

		[NUnit.Framework.Test]
		public void ValidateInitializedComponentIsPresent()
		{
			_defaultEntity.AddComponentA();

			AssertHasComponentA(_defaultEntity);
		}

		[NUnit.Framework.Test]
		public void CannotRemoveNonExistentComponentFromEntity()
		{
			Assert.Throws<EntityDoesNotHaveComponentException>(() => _defaultEntity.RemoveComponentA());
		}

		[NUnit.Framework.Test]
		public void ReplacingNonExistentComponentAddsComponent()
		{
			_defaultEntity.ReplaceComponentA(Component.A);

			AssertHasComponentA(_defaultEntity);
		}

		#endregion

		#region Adding Components

		[NUnit.Framework.Test]
		public void CannotAddDuplicateComponentToEntity()
		{
			_defaultEntity.AddComponentA();

			Assert.Throws<EntityAlreadyHasComponentException>(() => _defaultEntity.AddComponentA());
		}

		[NUnit.Framework.Test]
		public void ComponentCanBeRemovedAtIndex()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.RemoveComponentA();

			AssertHasNotComponentA(_defaultEntity);
		}

		[NUnit.Framework.Test]
		public void ExistingComponentCanBeReplaced()
		{
			var component = new ComponentA();
			_defaultEntity.ReplaceComponentA(component);

			AssertHasComponentA(_defaultEntity, component);
		}

		[NUnit.Framework.Test]
		public void CheckForMultipleComponentsByIndices()
		{
			_defaultEntity.AddComponentA();

			// Check for multiple all
			Assert.IsFalse(_defaultEntity.HasComponents(_indicesAB));

			// Check for multiple some
			Assert.IsTrue(_defaultEntity.HasAnyComponent(_indicesAB));
		}

		[NUnit.Framework.Test]
		public void ValidateGetAllComponents()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();

			var components = _defaultEntity.GetComponents();

			Assert.AreEqual(2, components.Length);
			Assert.Contains(Component.A, components);
			Assert.Contains(Component.B, components);
		}

		[NUnit.Framework.Test]
		public void ValidateHasMultipleComponentsByIndices()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();

			Assert.IsTrue(_defaultEntity.HasComponents(_indicesAB));
		}

		[NUnit.Framework.Test]
		public void ValidateAllComponentsRemoved()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();
			_defaultEntity.RemoveAllComponents();

			Assert.IsFalse(_defaultEntity.HasComponentA());
			Assert.IsFalse(_defaultEntity.HasComponentB());
			Assert.IsEmpty(_defaultEntity.GetComponents());
			Assert.IsEmpty(_defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ValidateToStringA()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();
			_defaultEntity.AddComponent(0, new StandardComponent());
			_defaultEntity.Retain(this);

			Assert.AreEqual(
				"Entity_0(EntitasRedux.Tests.StandardComponent, EntitasRedux.Tests.ComponentA, EntitasRedux.Tests.ComponentB)",
				_defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void ValidateToStringB()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();
			_defaultEntity.AddComponent(0, new NameAgeComponent { name = "Max", age = 42 });

			Assert.AreEqual(
				"Entity_0(NameAge(Max, 42), EntitasRedux.Tests.ComponentA, EntitasRedux.Tests.ComponentB)",
				_defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void ValidateToStringC()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();
			_defaultEntity.AddComponent(0, new MyNamespaceComponent());
			_defaultEntity.Retain(this);

			Assert.AreEqual(
				"Entity_0(EntitasRedux.Tests.MyNamespaceComponent, EntitasRedux.Tests.ComponentA, EntitasRedux.Tests.ComponentB)",
				_defaultEntity.ToString());
		}

		#endregion

		#region ComponentPool

		[NUnit.Framework.Test]
		public void ComponentPoolCanBeRetrieved()
		{
			var componentPool = _defaultEntity.GetComponentPool(CID.ComponentA);
			Assert.AreEqual(0, componentPool.Count);
		}

		[NUnit.Framework.Test]
		public void ComponentPoolRetrievedIsSameInstance()
		{
			Assert.AreEqual(
				_defaultEntity.GetComponentPool(CID.ComponentA),
				_defaultEntity.GetComponentPool(CID.ComponentA));
		}

		[NUnit.Framework.Test]
		public void ComponentPoolIsPushedInstanceWhenRemoved()
		{
			_defaultEntity.AddComponentA();
			var component = _defaultEntity.GetComponentA();
			_defaultEntity.RemoveComponentA();

			var componentPool = _defaultEntity.GetComponentPool(CID.ComponentA);
			Assert.AreEqual(1, componentPool.Count);
			Assert.AreEqual(component, componentPool.Pop());
		}

		[NUnit.Framework.Test]
		public void ComponentPoolCreatesNewComponentWhenEmpty()
		{
			var type = typeof(NameAgeComponent);
			var component = _defaultEntity.CreateComponent(1, type);

			Assert.AreEqual(type, component.GetType());

			var nameAgeComponent = ((NameAgeComponent)component);
			Assert.IsNull(nameAgeComponent.name);
			Assert.AreEqual(0, nameAgeComponent.age);
		}

		[NUnit.Framework.Test]
		public void ComponentPoolGetsNonPoolCreatedInstanceWhenRemoved()
		{
			var component = new NameAgeComponent();
			_defaultEntity.AddComponent(1, component);

			_defaultEntity.RemoveComponent(1);

			var newComponent = (NameAgeComponent)_defaultEntity.CreateComponent(1, typeof(NameAgeComponent));

			Assert.AreEqual(component, newComponent);
		}

		#endregion

		#region Events

		[NUnit.Framework.Test]
		public void OnComponentAddedInvoked()
		{
			_defaultEntity.OnComponentAdded += (entity, index, component) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, component);
			};
			_defaultEntity.OnComponentRemoved += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentReplaced += delegate { Assert.Fail(); };

			_defaultEntity.AddComponentA();

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentRemovedInvoked()
		{
			_defaultEntity.AddComponentA();

			_defaultEntity.OnComponentRemoved += (entity, index, component) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, component);
			};
			_defaultEntity.OnComponentAdded += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentReplaced += delegate { Assert.Fail(); };

			_defaultEntity.RemoveComponentA();

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentRemovedInvokedBeforePushingComponentToContext()
		{
			_defaultEntity.AddComponentA();

			_defaultEntity.OnComponentRemoved += (entity, index, component) =>
			{
				var newComponent = entity.CreateComponent(index, component.GetType());
				Assert.AreNotEqual(newComponent, component);
			};

			_defaultEntity.RemoveComponentA();
		}

		[NUnit.Framework.Test]
		public void OnComponentReplacedInvoked()
		{
			_defaultEntity.AddComponentA();
			var newComponentA = new ComponentA();

			_defaultEntity.OnComponentReplaced += (entity, index, previousComponent, newComponent) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(Component.A, previousComponent);;
				Assert.AreEqual(newComponentA, newComponent);
			};
			_defaultEntity.OnComponentAdded += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentRemoved += delegate { Assert.Fail(); };

			_defaultEntity.ReplaceComponentA(newComponentA);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentReplacedProvidesBothNewAndPreviousUniqueComponentInstance()
		{
			var prevComp = new ComponentA();
			var newComp = new ComponentA();

			_defaultEntity.OnComponentReplaced += (entity, index, previousComponent, newComponent) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
				Assert.AreEqual(prevComp, previousComponent);
				Assert.AreEqual(newComp, newComponent);
			};

			_defaultEntity.AddComponent(CID.ComponentA, prevComp);
			_defaultEntity.ReplaceComponent(CID.ComponentA, newComp);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentReplacedProvidesBothNewAndPreviousSameComponentInstance()
		{
			_defaultEntity.OnComponentReplaced += (entity, index, previousComponent, newComponent) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
				Assert.AreEqual(Component.A, previousComponent);
				Assert.AreEqual(Component.A, newComponent);
			};

			_defaultEntity.AddComponentA();
			_defaultEntity.ReplaceComponentA(Component.A);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void NonExistentComponentReplacedWithNullDoesNotDispatchAnything()
		{
			_defaultEntity.OnComponentAdded += delegate { Assert.Fail();};
			_defaultEntity.OnComponentReplaced += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentRemoved += delegate { Assert.Fail(); };

			_defaultEntity.ReplaceComponentA(null);
		}

		[NUnit.Framework.Test]
		public void OnComponentAddedInvokedWhenReplacingANonExistentComponent()
		{
			var newComponentA = new ComponentA();

			_defaultEntity.OnComponentAdded += (entity, index, component) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
				Assert.AreEqual(CID.ComponentA, index);
				Assert.AreEqual(newComponentA, component);
			};
			_defaultEntity.OnComponentReplaced += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentRemoved += delegate { Assert.Fail(); };

			_defaultEntity.ReplaceComponentA(newComponentA);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentRemovedInvokedWhenReplacingComponentWithNull()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.OnComponentRemoved += (entity, index, component) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(Component.A, component);
			};
			_defaultEntity.OnComponentAdded += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentReplaced += delegate { Assert.Fail(); };

			_defaultEntity.ReplaceComponentA(null);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentRemovedInvokedWhenRemovingMultipleComponents()
		{
			_defaultEntity.AddComponentA();
			_defaultEntity.AddComponentB();
			_defaultEntity.OnComponentRemoved += (entity, index, component) => _didDispatch += 1;
			_defaultEntity.RemoveAllComponents();

			Assert.AreEqual(2, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnDestroyInvokedWhenEntityDestroyed()
		{
			_defaultEntity.OnDestroyEntity += entity => _didDispatch += 1;
			_defaultEntity.Destroy();

			Assert.AreEqual(1, _didDispatch);
		}
		#endregion

		#region Reference Counting

		[NUnit.Framework.Test]
		public void EntityIsRetained()
		{
			Assert.AreEqual(0, _defaultEntity.RetainCount);

			_defaultEntity.Retain(this);

			Assert.AreEqual(1, _defaultEntity.RetainCount);

			if (_defaultEntity.AERC is SafeAERC safeAerc)
			{
				Assert.Contains(this, safeAerc.Owners.ToList());
			}
		}

		[NUnit.Framework.Test]
		public void EntityIsReleased()
		{
			_defaultEntity.Retain(this);
			_defaultEntity.Release(this);

			Assert.AreEqual(0, _defaultEntity.RetainCount);

			if (_defaultEntity.AERC is SafeAERC safeAerc)
			{
				Assert.IsFalse(safeAerc.Owners.Contains(this));
			}
		}

		[NUnit.Framework.Test]
		public void EntityThrowsWhenReleasedMoreThanHasBeenRetained()
		{
			_defaultEntity.Retain(this);
			_defaultEntity.Release(this);

			Assert.Throws<EntityIsNotRetainedByOwnerException>(() => _defaultEntity.Release(this));
		}

		[NUnit.Framework.Test]
		public void EntityThrowsWhenRetainedTwiceWithSameOwner()
		{
			_defaultEntity.Retain(this);

			Assert.Throws<EntityIsAlreadyRetainedByOwnerException>(() => _defaultEntity.Retain(this));
		}

		[NUnit.Framework.Test]
		public void EntityThrowsWhenReleasedByUnknownOwner()
		{
			var unknownOwner = new object();
			_defaultEntity.Retain(this);
			_defaultEntity.Release(this);

			Assert.Throws<EntityIsNotRetainedByOwnerException>(() => _defaultEntity.Release(unknownOwner));
		}

		[NUnit.Framework.Test]
		public void EntityThrowsWhenReleasingWithOwnerWhoDoesNotRetainEntityAnymore()
		{
			var owner1 = new object();
			var owner2 = new object();
			_defaultEntity.Retain(owner1);
			_defaultEntity.Retain(owner2);
			_defaultEntity.Release(owner2);

			Assert.Throws<EntityIsNotRetainedByOwnerException>(() => _defaultEntity.Release(owner2));
		}

		[NUnit.Framework.Test]
		public void OnEntityReleasedIsNotInvokedWhenEntityIsRetained()
		{
			_defaultEntity.OnEntityReleased += delegate { Assert.Fail();};
			_defaultEntity.Retain(this);
		}

		[NUnit.Framework.Test]
		public void OnEntityReleasedInvokedWhenEntityIsRetainedAndReleased()
		{
			_defaultEntity.OnEntityReleased += entity =>
			{
				_didDispatch += 1;

				Assert.AreEqual(_defaultEntity, entity);
			};

			_defaultEntity.Retain(this);
			_defaultEntity.Release(this);

			Assert.AreEqual(1, _didDispatch);

		}

		#endregion

		#region Internal Caching

		[NUnit.Framework.Test]
		public void ComponentsAreCached()
		{
			SetupComponentCache();

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ComponentCacheInvalidatedWhenNewComponentAdded()
		{
			SetupComponentCache();

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());

			_defaultEntity.AddComponentB();

			Assert.AreNotEqual(_componentCache, _defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ComponentCacheInvalidatedWhenComponentRemoved()
		{
			_defaultEntity.AddComponentB();

			SetupComponentCache();

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());

			_defaultEntity.RemoveComponentB();

			Assert.AreNotEqual(_componentCache, _defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ComponentCacheInvalidatedWhenComponentReplaced()
		{
			SetupComponentCache();

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());

			_defaultEntity.ReplaceComponentA(new ComponentA());

			Assert.AreNotEqual(_componentCache, _defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ComponentCacheUpdatedWhenComponentReplacedWithSameInstance()
		{
			SetupComponentCache();

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());

			_defaultEntity.ReplaceComponentA(Component.A);

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ComponentCacheInvalidatedWhenAllComponentsRemoved()
		{
			SetupComponentCache();

			Assert.AreEqual(_componentCache, _defaultEntity.GetComponents());

			_defaultEntity.RemoveAllComponents();

			Assert.AreNotEqual(_componentCache, _defaultEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void ComponentIndicesAreCached()
		{
			SetupComponentIndicesCache();

			Assert.AreEqual(_componentIndicesCache, _defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ComponentIndicesCacheInvalidatedWhenNewComponentAdded()
		{
			SetupComponentIndicesCache();

			_defaultEntity.AddComponentB();

			Assert.AreNotEqual(_componentIndicesCache, _defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ComponentIndicesCacheInvalidatedWhenComponentRemoved()
		{
			SetupComponentIndicesCache();

			_defaultEntity.RemoveComponentA();

			Assert.AreNotEqual(_componentIndicesCache, _defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ComponentIndicesCacheNotUpdatedWhenComponentReplaced()
		{
			SetupComponentIndicesCache();

			_defaultEntity.ReplaceComponentA(new ComponentA());

			Assert.AreEqual(_componentIndicesCache, _defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ComponentIndicesCacheInvalidatedWhenNewComponentReplaced()
		{
			SetupComponentIndicesCache();

			_defaultEntity.ReplaceComponentC(new ComponentC());

			Assert.AreNotEqual(_componentIndicesCache, _defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void ComponentIndicesCacheInvalidatedWhenAllComponentRemoved()
		{
			SetupComponentIndicesCache();

			_defaultEntity.RemoveAllComponents();

			Assert.AreNotEqual(_componentIndicesCache, _defaultEntity.GetComponentIndices());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionIsCached()
		{
			SetupDescriptionCache();

			Assert.AreEqual(_description, _defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheInvalidatedWhenNewComponentAdded()
		{
			SetupDescriptionCache();

			_defaultEntity.AddComponentB();

			Assert.AreNotEqual(_description, _defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheInvalidatedWhenComponentRemoved()
		{
			SetupDescriptionCache();

			_defaultEntity.RemoveComponentA();

			Assert.AreNotEqual(_description, _defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheNotUpdatedWhenComponentReplaced()
		{
			SetupDescriptionCache();

			_defaultEntity.ReplaceComponentA(new ComponentA());

			Assert.AreEqual(_description, _defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheInvalidatedWhenAllComponentRemoved()
		{
			_defaultEntity.AddComponentB();

			SetupDescriptionCache();

			_defaultEntity.RemoveAllComponents();

			Assert.AreNotEqual(_description, _defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheNotUpdatedWhenReleased()
		{
			_defaultEntity.Retain(this);
			_defaultEntity.Retain(new object());
			SetupDescriptionCache();

			_defaultEntity.Release(this);

			Assert.AreEqual(_description, _defaultEntity.ToString());
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheNotUpdatedWhenOnEntityReleased()
		{
			_defaultEntity.Retain(this);
			SetupDescriptionCache();

			_defaultEntity.OnEntityReleased += entity =>
			{
				Assert.AreEqual(_description, entity.ToString());
			};

			_defaultEntity.Release(this);
		}

		[NUnit.Framework.Test]
		public void EntityDescriptionCacheInvalidatedWhenMultipleComponentsRemoved()
		{
			SetupDescriptionCache();

			_defaultEntity.RemoveAllComponents();

			Assert.AreNotEqual(_description, _defaultEntity.ToString());
		}

		#endregion

		#region Helpers

		private void AssertHasComponentA(MyTestEntity e, IComponent componentA = null)
		{
			if (componentA == null)
			{
				componentA = Component.A;
			}

			Assert.AreEqual(componentA, e.GetComponentA());

			var components = e.GetComponents();

			Assert.AreEqual(1, components.Length);
			Assert.Contains(componentA, components);

			var indices = e.GetComponentIndices();

			Assert.AreEqual(1, indices.Length);
			Assert.Contains(CID.ComponentA, indices);

			Assert.IsTrue(e.HasComponentA());
			Assert.IsTrue(e.HasComponents(_indicesA));
			Assert.IsTrue(e.HasAnyComponent(_indicesA));
		}

		private void AssertHasNotComponentA(MyTestEntity e)
		{
			var components = e.GetComponents();

			Assert.AreEqual(0, components.Length);

			var indices = e.GetComponentIndices();

			Assert.AreEqual(0, indices.Length);
			Assert.IsFalse(e.HasComponentA());
			Assert.IsFalse(e.HasComponents(_indicesA));
			Assert.IsFalse(e.HasAnyComponent(_indicesA));
		}

		private void SetupComponentCache()
		{
			_defaultEntity.AddComponentA();
			_componentCache = _defaultEntity.GetComponents();
		}

		private void SetupComponentIndicesCache()
		{
			_defaultEntity.AddComponentA();
			_componentIndicesCache = _defaultEntity.GetComponentIndices();
		}

		private void SetupDescriptionCache()
		{
			_defaultEntity.AddComponentA();
			_description = _defaultEntity.ToString();
		}

		#endregion
	}
}
