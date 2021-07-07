public sealed class TestEventSystems : Feature
{
	public TestEventSystems(Contexts contexts)
	{
		Add(new AnyBaseAddedEventSystem(contexts)); // priority: 0
		Add(new TestAnyEventToGenerateAddedEventSystem(contexts)); // priority: 0
		Add(new AnyFlagEventRemovedEventSystem(contexts)); // priority: 0
		Add(new AnyMixedEventAddedEventSystem(contexts)); // priority: 0
		Add(new MixedEventAddedEventSystem(contexts)); // priority: 0
		Add(new TestAnyMultipleContextStandardEventAddedEventSystem(contexts)); // priority: 0
		Add(new AnyStandardEventAddedEventSystem(contexts)); // priority: 0
		Add(new AnyUniqueEventAddedEventSystem(contexts)); // priority: 0
		Add(new FlagEntityEventAddedEventSystem(contexts)); // priority: 1
		Add(new TestAnyMultipleEventsStandardEventAddedEventSystem(contexts)); // priority: 1
		Add(new StandardEntityEventRemovedEventSystem(contexts)); // priority: 1
		Add(new TestMultipleEventsStandardEventRemovedEventSystem(contexts)); // priority: 2
	}
}
