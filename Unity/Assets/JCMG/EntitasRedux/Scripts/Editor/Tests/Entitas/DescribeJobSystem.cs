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
using NUnit.Framework;

namespace EntitasRedux.Tests
{
	[TestFixture]
	internal sealed class DescribeJobSystem
	{
		private TestContext ctx;

		[SetUp]
		public void Setup()
		{
			ctx = new TestContext();
		}

		[NUnit.Framework.Test]
		public void EntityIsProcessed()
		{
			var system = new TestJobSystem(ctx, 2);
			var e = ctx.CreateEntity();
			e.AddNameAge("e", -1);
			system.Execute();

			Assert.AreEqual("e-Processed", e.NameAge.name);
		}

		[NUnit.Framework.Test]
		[Ignore("This test was ignored in the original source and is not currently passing.")]
		public void AllEntitiesAreProcessedWhenCountIsDividableByNumThreads()
		{
			var system = new TestJobSystem(ctx, 2);
			for (int i = 0; i < 4; i++)
			{
				var e = ctx.CreateEntity();
				e.AddNameAge("e" + i, -1);
			}

			system.Execute();

			var entities = ctx.GetEntities();

			Assert.AreEqual(4, entities.Length);

			for (int i = 0; i < entities.Length; i++)
			{
				Assert.AreEqual("e" + i + "-Processed", entities[i].NameAge.name);
			}

			Assert.AreEqual(entities[1].NameAge.age, entities[0].NameAge.age);
			Assert.AreEqual(entities[3].NameAge.age, entities[2].NameAge.age);
			Assert.AreEqual(entities[2].NameAge.age, entities[0].NameAge.age);
		}

		[NUnit.Framework.Test]
		public void AllEntitiesAreProcessedWhenCountIsNotDividableByNumThreads()
		{
			var system = new TestJobSystem(ctx, 4);
			for (int i = 0; i < 103; i++)
			{
				var e = ctx.CreateEntity();
				e.AddNameAge("e" + i, -1);
			}

			system.Execute();

			var entities = ctx.GetEntities();

			Assert.AreEqual(103, entities.Length);

			for (int i = 0; i < entities.Length; i++)
			{
				Assert.AreEqual("e" + i + "-Processed", entities[i].NameAge.name);
			}
		}

		[NUnit.Framework.Test]
		public void JobSystemThrowsWhenThreadThrows()
		{
			var system = new TestJobSystem(ctx, 2);
			system.exception = new Exception("Test Exception");
			for (int i = 0; i < 10; i++)
			{
				var e = ctx.CreateEntity();
				e.AddNameAge("e" + i, -1);
			}

			Assert.Throws<Exception>(() => system.Execute());
		}

		[NUnit.Framework.Test]
		public void JobSystemCanRecoverFromException()
		{
			var system = new TestJobSystem(ctx, 2);
			system.exception = new Exception("Test Exception");
			for (int i = 0; i < 10; i++)
			{
				var e = ctx.CreateEntity();
				e.AddNameAge("e" + i, -1);
			}

			var didThrow = 0;
			try
			{
				system.Execute();
			}
			catch
			{
				didThrow += 1;
			}

			Assert.AreEqual(1, didThrow);

			system.exception = null;

			system.Execute();
		}
	}
}
