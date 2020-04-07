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
	internal class ContextTests
	{
		[NUnit.Framework.Test]
		public void EnsuresSameDeterministicOrderWhenGettingEntitiesAfterDestroyAllEntities()
		{
			var context = new Context<Entity>(1, () => new GameEntity());

			const int numEntities = 10;
			for (var i = 0; i < numEntities; i++)
			{
				context.CreateEntity();
			}

			var order1 = new int[numEntities];
			var entities1 = context.GetEntities();
			for (var i = 0; i < numEntities; i++)
			{
				order1[i] = entities1[i].CreationIndex;
			}

			context.DestroyAllEntities();
			context.ResetCreationIndex();

			for (var i = 0; i < numEntities; i++)
			{
				context.CreateEntity();
			}

			var order2 = new int[numEntities];
			var entities2 = context.GetEntities();
			for (var i = 0; i < numEntities; i++)
			{
				order2[i] = entities2[i].CreationIndex;
			}

			for (var i = 0; i < numEntities; i++)
			{
				var index1 = order1[i];
				var index2 = order2[i];

				Assert.AreEqual(index1, index2);
			}
		}
	}
}
