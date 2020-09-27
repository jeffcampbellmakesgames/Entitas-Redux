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

namespace EntitasRedux.Tests
{
	internal sealed class DescribeEntitasErrorMessages
	{
		static void printErrorMessage(Action action) {
			try {
				action();
			} catch(Exception exception) {
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.WriteLine("================================================================================");
				Console.WriteLine("Exception preview for: " + exception.GetType());
				Console.WriteLine("--------------------------------------------------------------------------------");
				Console.WriteLine(exception.Message);
				Console.WriteLine("================================================================================");
				Console.ResetColor();
			}
		}

		private MyTestContext _context;
		private MyTestEntity _entity;

		[SetUp]
		public void Setup()
		{
			_context = new MyTestContext();
			_entity = _context.CreateEntity();
		}

		#region Entity

		[NUnit.Framework.Test]
		public void CannotRetainEntityMoreThanOnceByASingleOwner()
		{
			var owner = new object();
			_entity.Retain(owner);

			Assert.Throws<EntityIsAlreadyRetainedByOwnerException>(() => _entity.Retain(owner));
		}

		[NUnit.Framework.Test]
		public void EntityCannotBeReleasedWithWrongOwner()
		{
			var owner = new object();

			Assert.Throws<EntityIsNotRetainedByOwnerException>(() => _entity.Release(owner));
		}

		#endregion

		#region Group

		[NUnit.Framework.Test]
		public void CannotRetrieveSingleEntityWhenMultipleExistFromGroup()
		{
			_context.CreateEntity().AddComponentA();
			_context.CreateEntity().AddComponentA();

			var matcher = (Matcher<MyTestEntity>)Matcher<MyTestEntity>.AllOf(MyTestComponentsLookup.ComponentA);
			var group = _context.GetGroup(matcher);

			Assert.Throws<GroupSingleEntityException<MyTestEntity>>(() => group.GetSingleEntity());
		}

		#endregion
	}
}
