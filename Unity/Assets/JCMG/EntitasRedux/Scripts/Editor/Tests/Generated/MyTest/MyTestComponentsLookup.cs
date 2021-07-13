using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class MyTestComponentsLookup
{
	public const int Array3d = 0;
	public const int CleanupDestroy = 1;
	public const int CleanupRemove = 2;
	public const int ComponentA = 3;
	public const int ComponentB = 4;
	public const int ComponentC = 5;
	public const int ComponentD = 6;
	public const int ComponentE = 7;
	public const int ComponentF = 8;
	public const int DeepCopy = 9;
	public const int EntityIndexNoContext = 10;
	public const int Inherited = 11;
	public const int MultiplePrimaryEntityIndices = 12;
	public const int NameAge = 13;
	public const int NoContext = 14;
	public const int Parent = 15;
	public const int PrimaryEntityIndex = 16;
	public const int ShallowCopy = 17;

	public const int TotalComponents = 18;

	public static readonly string[] ComponentNames =
	{
		"Array3d",
		"CleanupDestroy",
		"CleanupRemove",
		"ComponentA",
		"ComponentB",
		"ComponentC",
		"ComponentD",
		"ComponentE",
		"ComponentF",
		"DeepCopy",
		"EntityIndexNoContext",
		"Inherited",
		"MultiplePrimaryEntityIndices",
		"NameAge",
		"NoContext",
		"Parent",
		"PrimaryEntityIndex",
		"ShallowCopy"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(EntitasRedux.Tests.Array3dComponent),
		typeof(EntitasRedux.Tests.CleanupDestroyComponent),
		typeof(EntitasRedux.Tests.CleanupRemoveComponent),
		typeof(EntitasRedux.Tests.ComponentA),
		typeof(EntitasRedux.Tests.ComponentB),
		typeof(EntitasRedux.Tests.ComponentC),
		typeof(EntitasRedux.Tests.ComponentD),
		typeof(EntitasRedux.Tests.ComponentE),
		typeof(EntitasRedux.Tests.ComponentF),
		typeof(EntitasRedux.Tests.DeepCopyComponent),
		typeof(EntitasRedux.Tests.EntityIndexNoContextComponent),
		typeof(EntitasRedux.Tests.InheritedComponent),
		typeof(EntitasRedux.Tests.MultiplePrimaryEntityIndicesComponent),
		typeof(EntitasRedux.Tests.NameAgeComponent),
		typeof(EntitasRedux.Tests.NoContextComponent),
		typeof(EntitasRedux.Tests.ParentComponent),
		typeof(EntitasRedux.Tests.PrimaryEntityIndexComponent),
		typeof(EntitasRedux.Tests.ShallowCopyComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(EntitasRedux.Tests.Array3dComponent), 0 },
		{ typeof(EntitasRedux.Tests.CleanupDestroyComponent), 1 },
		{ typeof(EntitasRedux.Tests.CleanupRemoveComponent), 2 },
		{ typeof(EntitasRedux.Tests.ComponentA), 3 },
		{ typeof(EntitasRedux.Tests.ComponentB), 4 },
		{ typeof(EntitasRedux.Tests.ComponentC), 5 },
		{ typeof(EntitasRedux.Tests.ComponentD), 6 },
		{ typeof(EntitasRedux.Tests.ComponentE), 7 },
		{ typeof(EntitasRedux.Tests.ComponentF), 8 },
		{ typeof(EntitasRedux.Tests.DeepCopyComponent), 9 },
		{ typeof(EntitasRedux.Tests.EntityIndexNoContextComponent), 10 },
		{ typeof(EntitasRedux.Tests.InheritedComponent), 11 },
		{ typeof(EntitasRedux.Tests.MultiplePrimaryEntityIndicesComponent), 12 },
		{ typeof(EntitasRedux.Tests.NameAgeComponent), 13 },
		{ typeof(EntitasRedux.Tests.NoContextComponent), 14 },
		{ typeof(EntitasRedux.Tests.ParentComponent), 15 },
		{ typeof(EntitasRedux.Tests.PrimaryEntityIndexComponent), 16 },
		{ typeof(EntitasRedux.Tests.ShallowCopyComponent), 17 }
	};

	/// <summary>
	/// Returns a component index based on the passed <paramref name="component"/> type; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="component"></param>
	public static int GetComponentIndex(IComponent component)
	{
		return GetComponentIndex(component.GetType());
	}

	/// <summary>
	/// Returns a component index based on the passed <paramref name="componentType"/>; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="componentType"></param>
	public static int GetComponentIndex(Type componentType)
	{
		return ComponentTypeToIndex.TryGetValue(componentType, out var index) ? index : -1;
	}
}
