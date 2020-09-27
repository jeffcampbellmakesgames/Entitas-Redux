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
	internal sealed class DescribeEntityIndex
	{
		private PrimaryEntityIndex<MyTestEntity, string> _primaryIndex;
		private EntityIndex<MyTestEntity, string> _index;
		private IContext<MyTestEntity> _context;
		private IGroup<MyTestEntity> _group;
		private MyTestEntity _entity;
		private NameAgeComponent _nameAgeComponent;
		private MyTestEntity _entity1;
		private MyTestEntity _entity2;

		private const string NAME = "Max";

		#region Activated PrimaryEntityIndex

		[NUnit.Framework.Test]
		public void PrimaryEntityIndexReturnsNullEntityForUnknownKey()
		{
			SetupActivatedPrimaryEntityIndex();

			Assert.IsNull(_primaryIndex.GetEntity("unknownKey"));
		}

		[NUnit.Framework.Test]
		public void PrimaryEntityIndexGetsEntityForKey()
		{
			SetupActivatedPrimaryEntityIndex();

			Assert.AreEqual(_entity, _primaryIndex.GetEntity(NAME));
		}

		[NUnit.Framework.Test]
		public void PrimaryEntityIndexRetainsEntity()
		{
			SetupActivatedPrimaryEntityIndex();

			Assert.AreEqual(3, _entity.RetainCount);
		}

		[NUnit.Framework.Test]
		public void PrimaryEntityIndexCanRetrieveExistingEntity()
		{
			SetupActivatedPrimaryEntityIndex();

			var newIndex = new PrimaryEntityIndex<MyTestEntity, string>(
				"TestIndex",
				_group,
				(e, c) =>
				{
					return c is NameAgeComponent nameAge
						? nameAge.name
						: ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name;
				});

			Assert.AreEqual(_entity, newIndex.GetEntity(NAME));
		}

		[NUnit.Framework.Test]
		public void PrimaryEntityIndexCanReleaseEntityWhenComponentGetsRemoved()
		{
			SetupActivatedPrimaryEntityIndex();

			_entity.RemoveComponent(MyTestComponentsLookup.ComponentA);

			Assert.IsNull(_primaryIndex.GetEntity(NAME));
			Assert.AreEqual(1, _entity.RetainCount); // Context
		}

		[NUnit.Framework.Test]
		public void PrimaryEntityIndexThrowsWhenAddingEntityForTheSameKey()
		{
			SetupActivatedPrimaryEntityIndex();

			var nameAgeComponent = new NameAgeComponent();
			nameAgeComponent.name = NAME;
			_entity = _context.CreateEntity();

			Assert.Throws<EntityIndexException>(() => _entity.AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent));
		}

		[NUnit.Framework.Test]
		public void ValidateActivePrimaryEntityIndexToString()
		{
			SetupActivatedPrimaryEntityIndex();

			Assert.AreEqual("PrimaryEntityIndex(TestIndex)", _primaryIndex.ToString());
		}

		#endregion

		#region Deactivated PrimaryEntityIndex

		[NUnit.Framework.Test]
		public void DeactivatedPrimaryEntityIndexIsCleared()
		{
			SetupDeactivatedPrimaryEntityIndex();

			Assert.IsNull(_primaryIndex.GetEntity(NAME));
			Assert.AreEqual(2, _entity.RetainCount);
		}

		[NUnit.Framework.Test]
		public void DeactivatedPrimaryEntityIndexDoesNotAddEntitiesAnymore()
		{
			SetupDeactivatedPrimaryEntityIndex();

			var nameAgeComponent = new NameAgeComponent();
			nameAgeComponent.name = NAME;
			_context.CreateEntity().AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent);

			Assert.IsNull(_primaryIndex.GetEntity(NAME));
		}

		#endregion

		#region Reactivated PrimaryEntityIndex

		[NUnit.Framework.Test]
		public void ReactivatedPrimaryEntityIndexHasExistingEntities()
		{
			SetupReactivatedPrimaryEntityIndex();

			Assert.AreEqual(_entity, _primaryIndex.GetEntity(NAME));
		}

		[NUnit.Framework.Test]
		public void ReactivatedPrimaryEntityIndexAddsNewEntities()
		{
			SetupReactivatedPrimaryEntityIndex();

			var nameAgeComponent = new NameAgeComponent();
			nameAgeComponent.name = "Jack";
			_entity = _context.CreateEntity();
			_entity.AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent);

			Assert.AreEqual(_entity, _primaryIndex.GetEntity("Jack"));
		}

		#endregion

		#region Multiple Keys PrimaryEntityIndex

		[NUnit.Framework.Test]
		public void MultipleKeyPrimaryEntityIndexRetainsEntity()
		{
			SetupMultipleKeyPrimaryEntityIndex();

			Assert.AreEqual(3, _entity.RetainCount);

			if (_entity.AERC is SafeAERC safeAerc)
			{
				Assert.Contains(_primaryIndex, safeAerc.Owners.ToList());
			}
		}

		[NUnit.Framework.Test]
		public void MultipleKeyPrimaryEntityIndexRetrievesEntity()
		{
			SetupMultipleKeyPrimaryEntityIndex();

			Assert.AreEqual(_entity, _primaryIndex.GetEntity(NAME + "1"));
			Assert.AreEqual(_entity, _primaryIndex.GetEntity(NAME + "2"));
		}

		[NUnit.Framework.Test]
		public void MultipleEntityIndexCanReleaseEntityWhenComponentGetsRemoved()
		{
			SetupMultipleKeyPrimaryEntityIndex();

			_entity.RemoveComponent(MyTestComponentsLookup.ComponentA);

			Assert.IsNull(_primaryIndex.GetEntity(NAME + "1"));
			Assert.IsNull(_primaryIndex.GetEntity(NAME + "2"));

			Assert.AreEqual(1, _entity.RetainCount); // Context

			if (_entity.AERC is SafeAERC safeAerc)
			{
				Assert.IsFalse(safeAerc.Owners.Contains(_primaryIndex));
			}
		}

		[NUnit.Framework.Test]
		public void MultipleEntityIndexCanRetrieveExistingEntityWhenReactivated()
		{
			SetupMultipleKeyPrimaryEntityIndex();

			_primaryIndex.Deactivate();
			_primaryIndex.Activate();

			Assert.AreEqual(_entity, _primaryIndex.GetEntity(NAME + "1"));
			Assert.AreEqual(_entity, _primaryIndex.GetEntity(NAME + "2"));
		}

		#endregion

		#region Activated EntityIndex

		[NUnit.Framework.Test]
		public void SingleKeyEntityIndexReturnsNullEntityForUnknownKey()
		{
			SetupActivatedSingleKeyEntityIndex();

			Assert.IsEmpty(_index.GetEntities("unknownKey"));
		}

		[NUnit.Framework.Test]
		public void MultipleEntitiesCanBeRetrievedForSingleKey()
		{
			SetupActivatedSingleKeyEntityIndex();

			var entities = _index.GetEntities(NAME);

			Assert.AreEqual(2, entities.Count);
			Assert.Contains(_entity1, entities.ToList());
			Assert.Contains(_entity2, entities.ToList());
		}

		[NUnit.Framework.Test]
		public void MultipleEntitiesRetained()
		{
			SetupActivatedSingleKeyEntityIndex();

			Assert.AreEqual(3, _entity1.RetainCount); // Context, Group, EntityIndex
			Assert.AreEqual(3, _entity1.RetainCount); // Context, Group, EntityIndex
		}

		[NUnit.Framework.Test]
		public void NewEntityIndexContainsExistingEntities()
		{
			SetupActivatedSingleKeyEntityIndex();

			var newIndex = new EntityIndex<MyTestEntity, string>("TestIndex", _group, (e, c) =>
			{
				return c is NameAgeComponent nameAge
					? nameAge.name
					: ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name;
			});

			Assert.AreEqual(2, newIndex.GetEntities(NAME).Count);
		}

		[NUnit.Framework.Test]
		public void EntityIndexReleasesAndRemovesEntityFromIndexWhenComponentRemoved()
		{
			SetupActivatedSingleKeyEntityIndex();

			_entity1.RemoveComponent(MyTestComponentsLookup.ComponentA);

			Assert.AreEqual(1, _index.GetEntities(NAME).Count);
			Assert.AreEqual(1, _entity1.RetainCount); // Context
		}

		[NUnit.Framework.Test]
		public void EntityIndexToString()
		{
			SetupActivatedSingleKeyEntityIndex();

			Assert.AreEqual("EntityIndex(TestIndex)", _index.ToString());
		}

		#endregion

		#region Deactivated EntityIndex

		[NUnit.Framework.Test]
		public void DeactivatedEntityIndexIsCleared()
		{
			SetupDeactivatedSingleKeyEntityIndex();

			Assert.IsEmpty(_index.GetEntities(NAME));
			Assert.AreEqual(2, _entity1.RetainCount); // Context, Group
			Assert.AreEqual(2, _entity2.RetainCount); // Context, Group
		}

		[NUnit.Framework.Test]
		public void DeactivatedEntityIndexDoesNotAddEntitiesAnymore()
		{
			SetupDeactivatedSingleKeyEntityIndex();

			_context.CreateEntity().AddComponent(MyTestComponentsLookup.ComponentA, _nameAgeComponent);

			Assert.IsEmpty(_index.GetEntities(NAME));
		}

		#endregion

		#region Reactivated EntityIndex

		[NUnit.Framework.Test]
		public void ReactivatedEntityIndexHasExistingEntities()
		{
			SetupReactivatedSingleKeyEntityIndex();

			var entities = _index.GetEntities(NAME);

			Assert.AreEqual(2, entities.Count);
			Assert.Contains(_entity1, entities.ToList());
			Assert.Contains(_entity2, entities.ToList());
		}

		[NUnit.Framework.Test]
		public void ReactivatedEntityIndexAddsNewEntities()
		{
			SetupReactivatedSingleKeyEntityIndex();

			var entity3 = _context.CreateEntity();
			entity3.AddComponent(MyTestComponentsLookup.ComponentA, _nameAgeComponent);

			var entities = _index.GetEntities(NAME);

			Assert.AreEqual(3, entities.Count);
			Assert.Contains(_entity1, entities.ToList());
			Assert.Contains(_entity2, entities.ToList());
			Assert.Contains(entity3, entities.ToList());
		}

		#endregion

		#region Multiple Key EntityIndex

		[NUnit.Framework.Test]
		public void MultipleKeyEntityIndexRetainsEntities()
		{
			SetupMultipleKeyEntityIndex();

			Assert.AreEqual(3, _entity1.RetainCount);
			Assert.AreEqual(3, _entity2.RetainCount);

			if (_entity1.AERC is SafeAERC safeAerc1)
			{
				Assert.Contains(_index, safeAerc1.Owners.ToList());
			}

			if (_entity1.AERC is SafeAERC safeAerc2)
			{
				Assert.Contains(_index, safeAerc2.Owners.ToList());
			}
		}

		[NUnit.Framework.Test]
		public void MultipleKeyEntityIndexContainsEntities()
		{
			SetupMultipleKeyEntityIndex();

			Assert.AreEqual(1, _index.GetEntities("1").Count);
			Assert.AreEqual(2, _index.GetEntities("2").Count);
			Assert.AreEqual(1, _index.GetEntities("3").Count);

			Assert.AreEqual(_entity1, _index.GetEntities("1").First());
			Assert.Contains(_entity1, _index.GetEntities("2").ToList());
			Assert.Contains(_entity2, _index.GetEntities("2").ToList());
			Assert.AreEqual(_entity2, _index.GetEntities("3").First());
		}

		[NUnit.Framework.Test]
		public void MultipleKeyEntityIndexCanReleaseEntityWhenComponentGetsRemoved()
		{
			SetupMultipleKeyEntityIndex();

			_entity1.RemoveComponent(MyTestComponentsLookup.ComponentA);

			Assert.AreEqual(0, _index.GetEntities("1").Count);
			Assert.AreEqual(1, _index.GetEntities("2").Count);
			Assert.AreEqual(1, _index.GetEntities("3").Count);

			Assert.AreEqual(1, _entity1.RetainCount);
			Assert.AreEqual(3, _entity2.RetainCount);

			if (_entity1.AERC is SafeAERC safeAerc1)
			{
				Assert.IsFalse(safeAerc1.Owners.Contains(_index));
			}

			if (_entity2.AERC is SafeAERC safeAerc2)
			{
				Assert.Contains(_index, safeAerc2.Owners.ToList());
			}
		}

		[NUnit.Framework.Test]
		public void MultipleKeyEntityIndexContainsExistingEntities()
		{
			SetupMultipleKeyEntityIndex();

			_index.Deactivate();
			_index.Activate();

			Assert.AreEqual(_entity1, _index.GetEntities("1").First());
			Assert.Contains(_entity1, _index.GetEntities("2").ToList());
			Assert.Contains(_entity2, _index.GetEntities("2").ToList());
			Assert.AreEqual(_entity2, _index.GetEntities("3").First());
		}

		#endregion

		#region Indexing Multiple Components

		[NUnit.Framework.Test]
		public void IndexGetsLastComponentThatTriggeredAddingEntityToGroup()
		{
			_context = new MyTestContext();

			IComponent receivedComponent = null;

			_group = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA, MyTestComponentsLookup.ComponentB));
			_index = new EntityIndex<MyTestEntity, string>("TestIndex", _group, (e, c) =>
			{
				receivedComponent = c;
				return ((NameAgeComponent)c).name;
			});

			var nameAgeComponent1 = new NameAgeComponent();
			nameAgeComponent1.name = "Max";

			var nameAgeComponent2 = new NameAgeComponent();
			nameAgeComponent2.name = "Jack";

			var entity = _context.CreateEntity();
			entity.AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent1);
			entity.AddComponent(MyTestComponentsLookup.ComponentB, nameAgeComponent2);

			Assert.AreEqual(nameAgeComponent2, receivedComponent);
		}

		[NUnit.Framework.Test]
		public void IndexOfMultipleComponentsWorksWithNoneOf()
		{
			_context = new MyTestContext();

			var receivedComponents = new List<IComponent>();

			var nameAgeComponent1 = new NameAgeComponent();
			nameAgeComponent1.name = "Max";

			var nameAgeComponent2 = new NameAgeComponent();
			nameAgeComponent2.name = "Jack";

			_group = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA).NoneOf(MyTestComponentsLookup.ComponentB));
			_index = new EntityIndex<MyTestEntity, string>("TestIndex", _group, (e, c) =>
			{
				receivedComponents.Add(c);

				if (c == nameAgeComponent1)
				{
					return ((NameAgeComponent)c).name;
				}

				return ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name;
			});

			var entity = _context.CreateEntity();
			entity.AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent1);
			entity.AddComponent(MyTestComponentsLookup.ComponentB, nameAgeComponent2);

			Assert.AreEqual(2, receivedComponents.Count);
			Assert.AreEqual(nameAgeComponent1, receivedComponents[0]);
			Assert.AreEqual(nameAgeComponent2, receivedComponents[1]);
		}

		#endregion

		#region Helpers

		public void SetupActivatedPrimaryEntityIndex()
		{
			_context = new MyTestContext();
			_group = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA));
			_primaryIndex = new PrimaryEntityIndex<MyTestEntity, string>(
				"TestIndex",
				_group,
				(e, c) =>
				{
					return c is NameAgeComponent nameAge
						? nameAge.name
						: ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name;
				});

			var nameAgeComponent = new NameAgeComponent();
			nameAgeComponent.name = NAME;

			_entity = _context.CreateEntity();
			_entity.AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent);
		}

		public void SetupDeactivatedPrimaryEntityIndex()
		{
			SetupActivatedPrimaryEntityIndex();

			_primaryIndex.Deactivate();
		}

		public void SetupReactivatedPrimaryEntityIndex()
		{
			SetupDeactivatedPrimaryEntityIndex();

			_primaryIndex.Activate();
		}

		public void SetupMultipleKeyPrimaryEntityIndex()
		{
			_context = new MyTestContext();
			_group = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA));
			_primaryIndex = new PrimaryEntityIndex<MyTestEntity, string>("TestIndex", _group, (e, c) =>
			{
				return c is NameAgeComponent nameAge
					? new[] { nameAge.name + "1", nameAge.name + "2" }
					: new[] { ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name + "1", ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name + "2" };
			});

			var nameAgeComponent = new NameAgeComponent();
			nameAgeComponent.name = NAME;
			_entity = _context.CreateEntity();
			_entity.AddComponent(MyTestComponentsLookup.ComponentA, nameAgeComponent);
		}

		public void SetupActivatedSingleKeyEntityIndex()
		{
			_context = new MyTestContext();
			_group = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA));
			_index = new EntityIndex<MyTestEntity, string>("TestIndex", _group, (e, c) =>
			{
				return c is NameAgeComponent nameAge
					? nameAge.name
					: ((NameAgeComponent)e.GetComponent(MyTestComponentsLookup.ComponentA)).name;
			});

			_nameAgeComponent = new NameAgeComponent();
			_nameAgeComponent.name = NAME;
			_entity1 = _context.CreateEntity();
			_entity1.AddComponent(MyTestComponentsLookup.ComponentA, _nameAgeComponent);
			_entity2 = _context.CreateEntity();
			_entity2.AddComponent(MyTestComponentsLookup.ComponentA, _nameAgeComponent);
		}

		public void SetupDeactivatedSingleKeyEntityIndex()
		{
			SetupActivatedSingleKeyEntityIndex();

			_index.Deactivate();
		}

		public void SetupReactivatedSingleKeyEntityIndex()
		{
			SetupDeactivatedSingleKeyEntityIndex();

			_index.Activate();
		}

		public void SetupMultipleKeyEntityIndex()
		{
			_context = new MyTestContext();
			_group = _context.GetGroup(Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA));
			_index = new EntityIndex<MyTestEntity, string>("TestIndex", _group, (e, c) =>
			{
				return e == _entity1
					? new[] { "1", "2" }
					: new[] { "2", "3" };
			});

			_entity1 = _context.CreateEntity();
			_entity1.AddComponentA();
			_entity2 = _context.CreateEntity();
			_entity2.AddComponentA();
		}

		#endregion
	}
}
