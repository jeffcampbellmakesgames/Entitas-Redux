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
using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	[CustomEntityIndex(typeof(TestContext))]
	public class MyCustomEntityIndex : EntityIndex<TestEntity, IntVector2> {

		static readonly List<IntVector2> _cachedList = new List<IntVector2>();

		public MyCustomEntityIndex(TestContext context)
			: base(
				"MyCustomEntityIndex",
				context.GetGroup(Matcher<TestEntity>.AllOf(TestMatcher.Position, TestMatcher.Size)),
				(e, c) => {
					var position = c is PositionComponent ? (PositionComponent)c : e.Position;
					var size = c is SizeComponent ? (SizeComponent)c : e.Size;

					_cachedList.Clear();
					for (int x = position.x; x < position.x + size.width; x++) {
						for (int y = position.y; y < position.y + size.height; y++) {
							_cachedList.Add(new IntVector2(x, y));
						}
					}

					return _cachedList.ToArray();
				}
			) {
		}

		[EntityIndexGetMethod]
		public HashSet<TestEntity> GetEntitiesWithPosition(IntVector2 position) {
			return GetEntities(position);
		}

		[EntityIndexGetMethod]
		public HashSet<TestEntity> GetEntitiesWithPosition2(IntVector2 position, IntVector2 size) {
			return GetEntities(position);
		}
	}

	[Test]
	public class PositionComponent : IComponent {
		public int x;
		public int y;
	}

	[Test]
	public class SizeComponent : IComponent {
		public int width;
		public int height;
	}

	public struct IntVector2 : IEquatable<IntVector2> {

		public int x;
		public int y;

		public IntVector2(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public bool Equals(IntVector2 other) {
			return other.x == x && other.y == y;
		}
	}
}
