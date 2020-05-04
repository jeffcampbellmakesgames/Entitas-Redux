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

namespace EntitasRedux.Tests
{
	// These classes are used to simplify some unit tests by focusing more on how components are handled rather
	// than the components themselves.

	public class ComponentA : IComponent {}
	public class ComponentB : IComponent {}
	public class ComponentC : IComponent {}
	public class ComponentD : IComponent {}
	public class ComponentE : IComponent {}
	public class ComponentF : IComponent {}

	public static class Component {
		public static readonly ComponentA A = new ComponentA();
		public static readonly ComponentB B = new ComponentB();
		public static readonly ComponentC C = new ComponentC();
	}

	public static class CID {

		public const int ComponentA = 1;
		public const int ComponentB = 2;
		public const int ComponentC = 3;
		public const int ComponentD = 4;

		public const int TotalComponents = 5;
	}

	public static class EntityTestExtensions {

		public static MyTestEntity AddComponentA(this IEntity e) { e.AddComponent(CID.ComponentA, Component.A); return (MyTestEntity)e; }
		public static MyTestEntity AddComponentB(this IEntity e) { e.AddComponent(CID.ComponentB, Component.B); return (MyTestEntity)e; }
		public static MyTestEntity AddComponentC(this IEntity e) { e.AddComponent(CID.ComponentC, Component.C); return (MyTestEntity)e; }

		public static bool HasComponentA(this IEntity e) { return e.HasComponent(CID.ComponentA); }
		public static bool HasComponentB(this IEntity e) { return e.HasComponent(CID.ComponentB); }
		public static bool HasComponentC(this IEntity e) { return e.HasComponent(CID.ComponentC); }

		public static MyTestEntity RemoveComponentA(this IEntity e) { e.RemoveComponent(CID.ComponentA); return (MyTestEntity)e; }
		public static MyTestEntity RemoveComponentB(this IEntity e) { e.RemoveComponent(CID.ComponentB); return (MyTestEntity)e; }
		public static MyTestEntity RemoveComponentC(this IEntity e) { e.RemoveComponent(CID.ComponentC); return (MyTestEntity)e; }

		public static ComponentA GetComponentA(this IEntity e) { return (ComponentA)e.GetComponent(CID.ComponentA); }
		public static ComponentB GetComponentB(this IEntity e) { return (ComponentB)e.GetComponent(CID.ComponentB); }
		public static ComponentC GetComponentC(this IEntity e) { return (ComponentC)e.GetComponent(CID.ComponentC); }

		public static MyTestEntity ReplaceComponentA(this IEntity e, ComponentA component) { e.ReplaceComponent(CID.ComponentA, component); return (MyTestEntity)e; }
		public static MyTestEntity ReplaceComponentB(this IEntity e, ComponentB component) { e.ReplaceComponent(CID.ComponentB, component); return (MyTestEntity)e; }
		public static MyTestEntity ReplaceComponentC(this IEntity e, ComponentC component) { e.ReplaceComponent(CID.ComponentC, component); return (MyTestEntity)e; }
	}
}
