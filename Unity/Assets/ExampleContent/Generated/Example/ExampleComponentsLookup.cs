using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class ExampleComponentsLookup
{
	public const int AnCleanupDestroyEntity = 0;
	public const int AnCleanupRemove = 1;

	public const int TotalComponents = 2;

	public static readonly string[] ComponentNames =
	{
		"AnCleanupDestroyEntity",
		"AnCleanupRemove"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent),
		typeof(ExampleContent.VisualDebugging.AnCleanupRemoveComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent), 0 },
		{ typeof(ExampleContent.VisualDebugging.AnCleanupRemoveComponent), 1 }
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
