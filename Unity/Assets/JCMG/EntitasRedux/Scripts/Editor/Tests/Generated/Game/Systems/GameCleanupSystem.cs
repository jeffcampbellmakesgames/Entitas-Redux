using JCMG.EntitasRedux;

public class GameCleanupSystems : JCMG.EntitasRedux.Systems
{
	public GameCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Game;
		_cleanupSystems.Add(new DestroyGameEntitiesWithCleanupEventSystem(context));
	}

	public GameCleanupSystems(IContext<GameEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyGameEntitiesWithCleanupEventSystem(context));
	}
}
