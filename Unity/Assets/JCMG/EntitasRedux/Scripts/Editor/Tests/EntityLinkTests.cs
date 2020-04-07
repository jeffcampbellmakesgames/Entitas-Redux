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
using JCMG.EntitasRedux;
using NUnit.Framework;
using UnityEngine;

namespace EntitasRedux.Tests
{
	public class EntityLinkTests
	{
		private IContext<GameEntity> _context;
		private IEntity _entity;
		private GameObject _gameObject;
		private EntityLink _link;

		[SetUp]
		public void BeforeEach()
		{
			_context = new GameContext();
			_entity = _context.CreateEntity();
			_gameObject = new GameObject("TestGameObject");
			_link = _gameObject.AddComponent<EntityLink>();
		}

		[NUnit.Framework.Test]
		public void LinksEntityAndContextAndRetainsEntity()
		{
			// given
			var retainCount = _entity.RetainCount;

			// when
			_link.Link(_entity);

			// then
			Assert.AreSame(_entity, _link.Entity);
			Assert.AreEqual(retainCount + 1, _entity.RetainCount);
			#if !ENTITAS_FAST_AND_UNSAFE
			Assert.IsTrue(((SafeAERC)_entity.AERC).Owners.Contains(_link));
			#endif
		}

		[NUnit.Framework.Test]
		public void ThrowsWhenAlreadyLinked()
		{
			Assert.Throws(
				typeof(Exception),
				() =>
				{
					_link.Link(_entity);
					_link.Link(_entity);
				});
		}

		[NUnit.Framework.Test]
		public void UnlinksEntityReleasesEntity()
		{
			// given
			_link.Link(_entity);
			var retainCount = _entity.RetainCount;

			// when
			_link.Unlink();

			Assert.AreEqual(retainCount - 1, _entity.RetainCount);
			Assert.IsNull(_link.Entity);
			#if !ENTITAS_FAST_AND_UNSAFE
			Assert.IsFalse(((SafeAERC)_entity.AERC).Owners.Contains(_link));
			#endif
		}

		[NUnit.Framework.Test]
		public void ThrowsWhenAlreadyUnlinked()
		{
			Assert.Throws(
				typeof(Exception),
				() =>
				{
					_link.Unlink();
				});
		}

		[NUnit.Framework.Test]
		public void GetSameEntityLink()
		{
			Assert.AreSame(_link, _gameObject.GetEntityLink());
		}

		[NUnit.Framework.Test]
		public void AddsEntityLinkAndLinks()
		{
			// given
			var gameObject = new GameObject();
			var RetainCount = _entity.RetainCount;

			// when
			var link = gameObject.Link(_entity);

			// then
			Assert.AreSame(link, gameObject.GetEntityLink());
			Assert.AreSame(_entity, link.Entity);
			Assert.AreEqual(RetainCount + 1, _entity.RetainCount);
		}

		[NUnit.Framework.Test]
		public void Unlinks()
		{
			// given
			var gameObject = new GameObject();
			var link = gameObject.Link(_entity);
			var RetainCount = _entity.RetainCount;

			// when
			gameObject.Unlink();

			// then
			Assert.AreSame(link, gameObject.GetEntityLink());
			Assert.AreEqual(RetainCount - 1, _entity.RetainCount);
			Assert.IsNull(link.Entity);
		}

		[NUnit.Framework.Test]
		public void ReusesLink()
		{
			// given
			var gameObject = new GameObject();
			var link1 = gameObject.Link(_context.CreateEntity());
			gameObject.Unlink();

			// when
			var link2 = gameObject.Link(_entity);

			// then
			Assert.AreEqual(1, gameObject.GetComponents<EntityLink>().Length);
			Assert.AreSame(link1, link2);
			Assert.AreSame(_entity, link2.Entity);
		}

		[NUnit.Framework.Test]
		public void CanToString()
		{
			Assert.AreEqual("EntityLink(TestGameObject)", _link.ToString());
		}
	}
}
