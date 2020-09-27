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
using UnityEngine;

namespace EntitasRedux.Tests
{
	internal class DescribeEntity
	{
		private readonly int[] _indicesA = { MyTestComponentsLookup.ComponentA };
		private readonly int[] _indicesAB = { MyTestComponentsLookup.ComponentA, MyTestComponentsLookup.ComponentB };

		private MyTestContext _context;
		private MyTestEntity _defaultEntity;
		private MyTestEntity _originalEntity;
		private MyTestEntity _targetEntity;
		private NameAgeComponent _nameAge;
		private int _didDispatch;
		private IComponent[] _componentCache;
		private int[] _componentIndicesCache;
		private string _description;

		[SetUp]
		public void Setup()
		{
			_context = new MyTestContext();
			_defaultEntity = TestTools.CreateEntity();
			_originalEntity = _context.CreateEntity();
			_targetEntity = _context.CreateEntity();
			_nameAge = new NameAgeComponent
			{
				name = "Max", age = 42
			};
			_didDispatch = 0;
		}

		#region Default Context Info

		[NUnit.Framework.Test]
		public void EntityHasDefaultContextInfo()
		{
			Assert.AreEqual("No Context", _defaultEntity.ContextInfo.name);
			Assert.AreEqual(MyTestComponentsLookup.TotalComponents, _defaultEntity.ContextInfo.componentNames.Length);
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
			Assert.AreEqual(MyTestComponentsLookup.TotalComponents, _defaultEntity.TotalComponents);
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

		#region Copying Components

		[NUnit.Framework.Test]
		public void CopyingComponentToEntityAddsComponent()
		{
			var entity = _context.CreateEntity();
			var newComponent = CreateShallowComponent();

			Assert.IsFalse(entity.HasShallowCopy);

			entity.CopyShallowCopyTo(newComponent);

			Assert.IsTrue(entity.HasShallowCopy);

			// Value types
			Assert.AreEqual(42, entity.ShallowCopy.intValue);
			Assert.AreEqual("42", entity.ShallowCopy.strValue);
			Assert.AreEqual(Vector2.one, entity.ShallowCopy.vector2Value);

			// UnityEngine.Object
			Assert.IsNotNull(entity.ShallowCopy.testScriptableObject);

			// The collection instances should be distinct, but the contents should be the same.
			// Dictionary
			Assert.AreNotEqual(newComponent.dictValue.GetHashCode(), entity.ShallowCopy.dictValue.GetHashCode());
			Assert.AreEqual(newComponent.dictValue, entity.ShallowCopy.dictValue);

			// List
			Assert.AreNotEqual(newComponent.listValue.GetHashCode(), entity.ShallowCopy.listValue.GetHashCode());
			Assert.AreEqual(newComponent.listValue, entity.ShallowCopy.listValue);

			// IListInterface
			Assert.AreNotEqual(newComponent.listValue.GetHashCode(), entity.ShallowCopy.listValue.GetHashCode());
			Assert.AreEqual(newComponent.listValue, entity.ShallowCopy.listValue);

			// All UnityEngine.Object instances should be shallow copied
			Assert.AreEqual(newComponent.testScriptableObject, entity.ShallowCopy.testScriptableObject);
		}

		[NUnit.Framework.Test]
		public void CopyingComponentToEntityReplacesComponent()
		{
			var entity = _context.CreateEntity();
			entity.AddShallowCopy(0, "0", Vector2.zero, null, null, null, null);

			var newComponent = CreateShallowComponent();

			entity.CopyShallowCopyTo(newComponent);

			// Value types
			Assert.AreEqual(42, entity.ShallowCopy.intValue);
			Assert.AreEqual("42", entity.ShallowCopy.strValue);
			Assert.AreEqual(Vector2.one, entity.ShallowCopy.vector2Value);

			// UnityEngine.Object
			Assert.IsNotNull(entity.ShallowCopy.testScriptableObject);

			// The collection instances should be distinct, but the contents should be the same.
			// Dictionary
			Assert.AreNotEqual(newComponent.dictValue.GetHashCode(), entity.ShallowCopy.dictValue.GetHashCode());
			Assert.AreEqual(newComponent.dictValue, entity.ShallowCopy.dictValue);

			// List
			Assert.AreNotEqual(newComponent.listValue.GetHashCode(), entity.ShallowCopy.listValue.GetHashCode());
			Assert.AreEqual(newComponent.listValue, entity.ShallowCopy.listValue);

			// IListInterface
			Assert.AreNotEqual(newComponent.listValue.GetHashCode(), entity.ShallowCopy.listValue.GetHashCode());
			Assert.AreEqual(newComponent.listValue, entity.ShallowCopy.listValue);

			// All UnityEngine.Object instances should be shallow copied
			Assert.AreEqual(newComponent.testScriptableObject, entity.ShallowCopy.testScriptableObject);
		}

		[NUnit.Framework.Test]
		public void ExpectedMembersAreCopiedAsShallow()
		{
			var entity = _context.CreateEntity();

			// Add a version of the component to the entity
			entity.AddShallowCopy(0, "0", Vector2.zero, null, null, null, null);

			// Now copy this new component to the entity
			var newComponent = CreateShallowComponent();
			entity.CopyShallowCopyTo(newComponent);

			Assert.AreNotEqual(entity.ShallowCopy, newComponent);

			// All of these value types should be equal
			Assert.AreEqual(newComponent.intValue, entity.ShallowCopy.intValue);
			Assert.AreEqual(newComponent.strValue, entity.ShallowCopy.strValue);
			Assert.AreEqual(newComponent.vector2Value, entity.ShallowCopy.vector2Value);

			// The collection instances should be distinct, but the contents should be the same.
			// Dictionary
			Assert.AreNotEqual(newComponent.dictValue.GetHashCode(), entity.ShallowCopy.dictValue.GetHashCode());
			Assert.AreEqual(newComponent.dictValue, entity.ShallowCopy.dictValue);

			// List
			Assert.AreNotEqual(newComponent.listValue.GetHashCode(), entity.ShallowCopy.listValue.GetHashCode());
			Assert.AreEqual(newComponent.listValue, entity.ShallowCopy.listValue);

			// IListInterface
			Assert.AreNotEqual(newComponent.listValue.GetHashCode(), entity.ShallowCopy.listValue.GetHashCode());
			Assert.AreEqual(newComponent.listValue, entity.ShallowCopy.listValue);

			// All UnityEngine.Object instances should be shallow copied
			Assert.AreEqual(newComponent.testScriptableObject, entity.ShallowCopy.testScriptableObject);
		}

		[NUnit.Framework.Test]
		public void ExpectedMembersAreCopiedAsDeep()
		{
			var entity = _context.CreateEntity();
			var deepComponent = CreateDeepComponent();
			entity.CopyComponentTo(deepComponent);

			// Any cloneable object should be deep cloned
			Assert.AreNotEqual(deepComponent.value, entity.DeepCopy.value);

			// The dictionary keys should be the same, but the content's distinct
			Assert.AreNotEqual(deepComponent.dict.GetHashCode(), entity.DeepCopy.dict.GetHashCode());
			Assert.AreNotEqual(deepComponent.dict, entity.DeepCopy.dict);
			Assert.IsTrue(deepComponent.dict.Count == 1 && deepComponent.dict.Count == entity.DeepCopy.dict.Count);
			Assert.AreEqual(deepComponent.dict.Keys.ToArray()[0], entity.DeepCopy.dict.Keys.ToArray()[0]);
			Assert.AreNotEqual(deepComponent.dict.Values.ToArray()[0], entity.DeepCopy.dict.Values.ToArray()[0]);

			// The list and it's contents should not be the same
			Assert.AreNotEqual(deepComponent.list.GetHashCode(), entity.DeepCopy.list.GetHashCode());
			Assert.AreNotEqual(deepComponent.list, entity.DeepCopy.list);
		}

		#endregion

		#region Copying Entity To Another

		[NUnit.Framework.Test]
		public void EntityIsNotChangedIfOriginalDoesNotHaveComponents()
		{
			_originalEntity.CopyTo(_targetEntity);

			Assert.AreEqual(0, _originalEntity.CreationIndex);
			Assert.AreEqual(1, _targetEntity.CreationIndex);
			Assert.IsEmpty(_targetEntity.GetComponents());
		}

		[NUnit.Framework.Test]
		public void CopiesOfAllComponentsAddedToTargetEntity()
		{
			_originalEntity.AddComponentA();
			_originalEntity.AddComponent(MyTestComponentsLookup.NameAge, _nameAge);
			_originalEntity.CopyTo(_targetEntity);

			Assert.AreEqual(2, _targetEntity.GetComponents().Length);
			Assert.IsTrue(_targetEntity.HasComponentA());
			Assert.IsTrue(_targetEntity.HasNameAge);
			Assert.AreNotEqual(Component.A, _targetEntity.GetComponentA());
			Assert.AreNotEqual(_nameAge, _targetEntity.NameAge);

			var clonedComponent = (NameAgeComponent)_targetEntity.GetComponent(MyTestComponentsLookup.NameAge);

			Assert.AreEqual(_nameAge.name, clonedComponent.name);
			Assert.AreEqual(_nameAge.age, clonedComponent.age);
		}

		[NUnit.Framework.Test]
		public void ThrowsWhenTargetAlreadyHasComponent()
		{
			_originalEntity.AddComponentA();
			_originalEntity.AddComponent(MyTestComponentsLookup.ComponentB, _nameAge);
			var component = new NameAgeComponent();
			_targetEntity.AddComponent(MyTestComponentsLookup.ComponentB, component);

			Assert.Throws<EntityAlreadyHasComponentException>(() => _originalEntity.CopyTo(_targetEntity));
		}

		[NUnit.Framework.Test]
		public void ReplacesExistingComponentWhenOverwriteIsSet()
		{
			_originalEntity.AddComponentA();
			_originalEntity.AddComponent(MyTestComponentsLookup.NameAge, _nameAge);
			var component = new NameAgeComponent();
			_targetEntity.AddComponent(MyTestComponentsLookup.NameAge, component);
			_originalEntity.CopyTo(_targetEntity, true);

			var copy = _targetEntity.GetComponent(MyTestComponentsLookup.NameAge);

			Assert.AreNotEqual(_nameAge, copy);
			Assert.AreNotEqual(component, copy);

			Assert.AreEqual(_nameAge.name, ((NameAgeComponent)copy).name);
			Assert.AreEqual(_nameAge.age, ((NameAgeComponent)copy).age);
		}

		[NUnit.Framework.Test]
		public void OnlyAddsCopiesOfSpecifiedComponentsToTargetEntity()
		{
			_originalEntity.AddComponentA();
			_originalEntity.AddComponentB();
			_originalEntity.AddComponentC();
			_originalEntity.CopyTo(
				_targetEntity,
				false,
				MyTestComponentsLookup.ComponentB,
				MyTestComponentsLookup.ComponentC);

			Assert.AreEqual(2, _targetEntity.GetComponents().Length);
			Assert.IsTrue(_targetEntity.HasComponentB());
			Assert.IsTrue(_targetEntity.HasComponentC());
		}

		[NUnit.Framework.Test]
		public void UsesComponentPoolWhenCopyingComponents()
		{
			_originalEntity.AddComponentA();

			var component = new ComponentA();
			_targetEntity.GetComponentPool(MyTestComponentsLookup.ComponentA).Push(component);
			_originalEntity.CopyTo(_targetEntity);

			Assert.AreEqual(component, _targetEntity.GetComponentA());
		}

		#endregion

		#region ComponentPool

		[NUnit.Framework.Test]
		public void ComponentPoolCanBeRetrieved()
		{
			var componentPool = _defaultEntity.GetComponentPool(MyTestComponentsLookup.ComponentA);
			Assert.AreEqual(0, componentPool.Count);
		}

		[NUnit.Framework.Test]
		public void ComponentPoolRetrievedIsSameInstance()
		{
			Assert.AreEqual(
				_defaultEntity.GetComponentPool(MyTestComponentsLookup.ComponentA),
				_defaultEntity.GetComponentPool(MyTestComponentsLookup.ComponentA));
		}

		[NUnit.Framework.Test]
		public void ComponentPoolIsPushedInstanceWhenRemoved()
		{
			_defaultEntity.AddComponentA();
			var component = _defaultEntity.GetComponentA();
			_defaultEntity.RemoveComponentA();

			var componentPool = _defaultEntity.GetComponentPool(MyTestComponentsLookup.ComponentA);
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
				Assert.AreEqual(MyTestComponentsLookup.ComponentA, index);
				Assert.AreEqual(Component.A, component);
			};
			_defaultEntity.OnComponentRemoved += delegate { Assert.Fail(); };
			_defaultEntity.OnComponentReplaced += delegate { Assert.Fail(); };

			_defaultEntity.AddComponentA();

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentCopiedAddedInvokedWhenNonePresent()
		{
			var entity = _context.CreateEntity();
			entity.OnComponentAdded += (ent, index, component) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(entity, ent);
				Assert.AreEqual(MyTestComponentsLookup.Parent, index);
			};
			entity.OnComponentRemoved += delegate
			{
				Assert.Fail();
			};
			entity.OnComponentReplaced += delegate
			{
				Assert.Fail();
			};

			entity.AddParent(5);

			Assert.AreEqual(1, _didDispatch);
		}

		[NUnit.Framework.Test]
		public void OnComponentCopiedReplacedInvokedWhenComponentPresent()
		{
			var entity = _context.CreateEntity();
			entity.AddParent(5);
			var newParentComponent = new ParentComponent();

			entity.OnComponentReplaced += (ent, index, previousComponent, newComponent) =>
			{
				_didDispatch += 1;

				Assert.AreEqual(entity, ent);
				Assert.AreEqual(MyTestComponentsLookup.Parent, index);
			};
			entity.OnComponentAdded += delegate
			{
				Assert.Fail();
			};
			entity.OnComponentRemoved += delegate
			{
				Assert.Fail();
			};

			entity.CopyComponentTo(newParentComponent);

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
				Assert.AreEqual(MyTestComponentsLookup.ComponentA, index);
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
				Assert.AreEqual(MyTestComponentsLookup.ComponentA, index);
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

			_defaultEntity.AddComponent(MyTestComponentsLookup.ComponentA, prevComp);
			_defaultEntity.ReplaceComponent(MyTestComponentsLookup.ComponentA, newComp);

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
				Assert.AreEqual(MyTestComponentsLookup.ComponentA, index);
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

		private ShallowCopyComponent CreateShallowComponent()
		{
			var testScriptableObject = ScriptableObject.CreateInstance<TestScriptableObject>();
			return new ShallowCopyComponent
			{
				intValue = 42,
				strValue = "42",
				vector2Value = Vector2.one,
				testScriptableObject = testScriptableObject,
				dictValue = new Dictionary<int, TestScriptableObject>
				{
					{
						42, testScriptableObject
					}
				},
				listValue = new List<TestScriptableObject>
				{
					testScriptableObject
				},
				iListValue = new List<TestScriptableObject>()
				{
					testScriptableObject
				}
			};
		}

		private DeepCopyComponent CreateDeepComponent()
		{
			return new DeepCopyComponent
			{
				value = new CloneableObject(),
				dict = new Dictionary<CloneableObject, CloneableObject>
					{{new CloneableObject(), new CloneableObject()}},
				list = new List<CloneableObject> {new CloneableObject()}

			};
		}

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
			Assert.Contains(MyTestComponentsLookup.ComponentA, indices);

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
