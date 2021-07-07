public sealed class Test2EventSystems : Feature
{
	public Test2EventSystems(Contexts contexts)
	{
		Add(new Test2AnyEventToGenerateAddedEventSystem(contexts)); // priority: 0
		Add(new Test2AnyMultipleContextStandardEventAddedEventSystem(contexts)); // priority: 0
		Add(new Test2AnyMultipleEventsStandardEventAddedEventSystem(contexts)); // priority: 1
		Add(new Test2MultipleEventsStandardEventRemovedEventSystem(contexts)); // priority: 2
	}
}
