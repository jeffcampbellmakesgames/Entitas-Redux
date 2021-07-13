public sealed class VisualDebugEventSystems : Feature
{
	public VisualDebugEventSystems(Contexts contexts)
	{
		Add(new AnyMyEventAddedEventSystem(contexts)); // priority: 0
		Add(new AnyMyEventClassAddedEventSystem(contexts)); // priority: 0
	}
}
