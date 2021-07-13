using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class Test2ComponentsLookup
{
	public const int ClassToGenerate = 0;
	public const int EntityIndex = 1;
	public const int MultipleContextStandardEvent = 2;
	public const int MultipleEntityIndices = 3;
	public const int MultipleEventsStandardEvent = 4;
	public const int MyNamespace = 5;
	public const int NameAge = 6;
	public const int Test2Context = 7;
	public const int EventToGenerate = 8;
	public const int Test2AnyEventToGenerateAddedListener = 9;
	public const int Test2AnyMultipleContextStandardEventAddedListener = 10;
	public const int Test2AnyMultipleEventsStandardEventAddedListener = 11;
	public const int Test2MultipleEventsStandardEventRemovedListener = 12;
	public const int UniqueClassToGenerate = 13;

	public const int TotalComponents = 14;

	public static readonly string[] ComponentNames =
	{
		"ClassToGenerate",
		"EntityIndex",
		"MultipleContextStandardEvent",
		"MultipleEntityIndices",
		"MultipleEventsStandardEvent",
		"MyNamespace",
		"NameAge",
		"Test2Context",
		"EventToGenerate",
		"Test2AnyEventToGenerateAddedListener",
		"Test2AnyMultipleContextStandardEventAddedListener",
		"Test2AnyMultipleEventsStandardEventAddedListener",
		"Test2MultipleEventsStandardEventRemovedListener",
		"UniqueClassToGenerate"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ClassToGenerateComponent),
		typeof(EntitasRedux.Tests.EntityIndexComponent),
		typeof(EntitasRedux.Tests.MultipleContextStandardEventComponent),
		typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent),
		typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent),
		typeof(EntitasRedux.Tests.MyNamespaceComponent),
		typeof(EntitasRedux.Tests.NameAgeComponent),
		typeof(EntitasRedux.Tests.Test2ContextComponent),
		typeof(EventToGenerateComponent),
		typeof(Test2AnyEventToGenerateAddedListenerComponent),
		typeof(Test2AnyMultipleContextStandardEventAddedListenerComponent),
		typeof(Test2AnyMultipleEventsStandardEventAddedListenerComponent),
		typeof(Test2MultipleEventsStandardEventRemovedListenerComponent),
		typeof(UniqueClassToGenerateComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ClassToGenerateComponent), 0 },
		{ typeof(EntitasRedux.Tests.EntityIndexComponent), 1 },
		{ typeof(EntitasRedux.Tests.MultipleContextStandardEventComponent), 2 },
		{ typeof(EntitasRedux.Tests.MultipleEntityIndicesComponent), 3 },
		{ typeof(EntitasRedux.Tests.MultipleEventsStandardEventComponent), 4 },
		{ typeof(EntitasRedux.Tests.MyNamespaceComponent), 5 },
		{ typeof(EntitasRedux.Tests.NameAgeComponent), 6 },
		{ typeof(EntitasRedux.Tests.Test2ContextComponent), 7 },
		{ typeof(EventToGenerateComponent), 8 },
		{ typeof(Test2AnyEventToGenerateAddedListenerComponent), 9 },
		{ typeof(Test2AnyMultipleContextStandardEventAddedListenerComponent), 10 },
		{ typeof(Test2AnyMultipleEventsStandardEventAddedListenerComponent), 11 },
		{ typeof(Test2MultipleEventsStandardEventRemovedListenerComponent), 12 },
		{ typeof(UniqueClassToGenerateComponent), 13 }
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
