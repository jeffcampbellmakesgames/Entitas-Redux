using JCMG.EntitasRedux;

public class GameCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public GameCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Game;

	}
	#endif

	public GameCleanupSystems(IContext<GameEntity> context) : base()
	{

	}
}
