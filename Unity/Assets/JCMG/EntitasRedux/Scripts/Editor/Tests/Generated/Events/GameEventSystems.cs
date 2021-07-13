public sealed class GameEventSystems : Feature
{
	public GameEventSystems(Contexts contexts)
	{
		Add(new CleanupEventAddedEventSystem(contexts)); // priority: 0
	}
}
