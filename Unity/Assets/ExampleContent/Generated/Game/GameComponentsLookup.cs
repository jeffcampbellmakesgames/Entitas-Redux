using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class GameComponentsLookup
{
	public const int NoContext = 0;

	public const int TotalComponents = 1;

	public static readonly string[] ComponentNames =
	{
		"NoContext"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ExampleContent.VisualDebugging.NoContextComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ExampleContent.VisualDebugging.NoContextComponent), 0 }
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
