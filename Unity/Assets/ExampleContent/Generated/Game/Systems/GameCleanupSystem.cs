using JCMG.EntitasRedux;

public class GameCleanupSystems : JCMG.EntitasRedux.Systems
{
	public GameCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Game;

	}

	public GameCleanupSystems(IContext<GameEntity> context) : base()
	{

	}
}
