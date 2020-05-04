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

namespace JCMG.EntitasRedux
{
	public interface IEntity : IAERC
	{
		int TotalComponents { get; }
		int CreationIndex { get; }
		bool IsEnabled { get; }

		Stack<IComponent>[] ComponentPools { get; }
		ContextInfo ContextInfo { get; }
		IAERC AERC { get; }

		event EntityComponentChanged OnComponentAdded;
		event EntityComponentChanged OnComponentRemoved;
		event EntityComponentReplaced OnComponentReplaced;
		event EntityEvent OnEntityReleased;
		event EntityEvent OnDestroyEntity;

		void Initialize(int creationIndex,
		                int totalComponents,
		                Stack<IComponent>[] componentPools,
		                ContextInfo contextInfo = null,
		                IAERC aerc = null);

		void Reactivate(int creationIndex);

		void AddComponent(int index, IComponent component);
		void RemoveComponent(int index);
		void ReplaceComponent(int index, IComponent component);

		IComponent GetComponent(int index);
		IComponent[] GetComponents();
		int[] GetComponentIndices();

		bool HasComponent(int index);
		bool HasComponents(int[] indices);
		bool HasAnyComponent(int[] indices);

		void RemoveAllComponents();

		Stack<IComponent> GetComponentPool(int index);
		IComponent CreateComponent(int index, Type type);

		T CreateComponent<T>(int index)
			where T : new();

		void Destroy();
		void InternalDestroy();
		void RemoveAllOnEntityReleasedHandlers();
	}
}
