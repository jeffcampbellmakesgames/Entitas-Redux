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

namespace EntitasRedux.Tests.Extensions
{
	[TestFixture]
	internal sealed class DescribePublicMemberInfoEntityExtension
	{
		private IContext<MyTestEntity> _ctx;
		private MyTestEntity _entity;
		private MyTestEntity _target;
		private NameAgeComponent _nameAge;

		private const string IGNORE_REASON = "This test has been ignored as the extension method for " +
		                                     "PublicMemberInfo.CopyTo is slated for refactor and/or deprecation";

		[SetUp]
		public void Setup()
		{
			_ctx = new MyTestContext();
			_entity = _ctx.CreateEntity();
			_target = _ctx.CreateEntity();
			_nameAge = new NameAgeComponent
			{
				name = "Max", age = 42
			};
		}

		[NUnit.Framework.Test]
		[Ignore(IGNORE_REASON)]
		public void EntityIsNotChangedIfOriginalDoesNotHaveComponents()
		{
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable HAA0101 // Array allocation for params parameter
			_entity.CopyTo(_target);
#pragma warning restore HAA0101 // Array allocation for params parameter
#pragma warning restore CS0618 // Type or member is obsolete

			Assert.AreEqual(0, _entity.CreationIndex);
			Assert.AreEqual(1, _target.CreationIndex);
			Assert.IsEmpty(_target.GetComponents());
		}

		[NUnit.Framework.Test]
		[Ignore(IGNORE_REASON)]
		public void CopiesOfAllComponentsAddedToTargetEntity()
		{
			_entity.AddComponentA();
			_entity.AddComponent(CID.ComponentB, _nameAge);

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable HAA0101 // Array allocation for params parameter
			_entity.CopyTo(_target);
#pragma warning restore HAA0101 // Array allocation for params parameter
#pragma warning restore CS0618 // Type or member is obsolete

			Assert.AreEqual(2, _target.GetComponents().Length);
			Assert.IsTrue(_target.HasComponentA());
			Assert.IsTrue(_target.HasComponentB());
			Assert.AreNotEqual(Component.A, _target.GetComponentA());
			Assert.AreNotEqual(_nameAge, _target.GetComponent(CID.ComponentB));

			var clonedComponent = (NameAgeComponent)_target.GetComponent(CID.ComponentB);

			Assert.AreEqual(_nameAge.name, clonedComponent.name);
			Assert.AreEqual(_nameAge.age, clonedComponent.age);
		}

		[NUnit.Framework.Test]
		[Ignore(IGNORE_REASON)]
		public void ThrowsWhenTargetAlreadyHasComponent()
		{
			_entity.AddComponentA();
			_entity.AddComponent(CID.ComponentB, _nameAge);
			var component = new NameAgeComponent();
			_target.AddComponent(CID.ComponentB, component);

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable HAA0101 // Array allocation for params parameter
			Assert.Throws<EntityAlreadyHasComponentException>(() => _entity.CopyTo(_target));
#pragma warning restore HAA0101 // Array allocation for params parameter
#pragma warning restore CS0618 // Type or member is obsolete
		}

		[NUnit.Framework.Test]
		[Ignore(IGNORE_REASON)]
		public void ReplacesExistingComponentWhenOverwriteIsSet()
		{
			_entity.AddComponentA();
			_entity.AddComponent(CID.ComponentB, _nameAge);
			var component = new NameAgeComponent();
			_target.AddComponent(CID.ComponentB, component);

#pragma warning disable CS0618 // Type or member is obsolete
			_entity.CopyTo(_target, true);
#pragma warning restore CS0618 // Type or member is obsolete

			var copy = _target.GetComponent(CID.ComponentB);

			Assert.AreNotEqual(_nameAge, copy);
			Assert.AreNotEqual(component, copy);

			Assert.AreEqual(_nameAge.name, ((NameAgeComponent)copy).name);
			Assert.AreEqual(_nameAge.age, ((NameAgeComponent)copy).age);
		}

		[NUnit.Framework.Test]
		[Ignore(IGNORE_REASON)]
		public void OnlyAddsCopiesOfSpecifiedComponentsToTargetEntity()
		{
			_entity.AddComponentA();
			_entity.AddComponentB();
			_entity.AddComponentC();

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable HAA0101 // Array allocation for params parameter
			_entity.CopyTo(
				_target,
				false,
				CID.ComponentB,
				CID.ComponentC);
#pragma warning restore HAA0101 // Array allocation for params parameter
#pragma warning restore CS0618 // Type or member is obsolete

			Assert.AreEqual(2, _target.GetComponents().Length);
			Assert.IsTrue(_target.HasComponentB());
			Assert.IsTrue(_target.HasComponentC());
		}

		[NUnit.Framework.Test]
		[Ignore(IGNORE_REASON)]
		public void UsesComponentPoolWhenCopyingComponents()
		{
			_entity.AddComponentA();

			var component = new ComponentA();
			_target.GetComponentPool(CID.ComponentA).Push(component);

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable HAA0101 // Array allocation for params parameter
			_entity.CopyTo(_target);
#pragma warning restore HAA0101 // Array allocation for params parameter
#pragma warning restore CS0618 // Type or member is obsolete

			Assert.AreEqual(component, _target.GetComponentA());
		}
	}
}
