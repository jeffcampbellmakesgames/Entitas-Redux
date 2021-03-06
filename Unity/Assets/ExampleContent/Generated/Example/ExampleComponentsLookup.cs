//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class ExampleComponentsLookup
{
	public const int AnCleanupDestroyEntity = 0;
	public const int AnCleanupRemove = 1;
	public const int NoContext = 2;

	public const int TotalComponents = 3;

	public static readonly string[] ComponentNames =
	{
		"AnCleanupDestroyEntity",
		"AnCleanupRemove",
		"NoContext"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent),
		typeof(ExampleContent.VisualDebugging.AnCleanupRemoveComponent),
		typeof(ExampleContent.VisualDebugging.NoContextComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent), 0 },
		{ typeof(ExampleContent.VisualDebugging.AnCleanupRemoveComponent), 1 },
		{ typeof(ExampleContent.VisualDebugging.NoContextComponent), 2 }
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
