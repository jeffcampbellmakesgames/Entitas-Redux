using JCMG.EntitasRedux;

public class GameCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public GameCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Game);
	}
	#endif

	public GameCleanupFeature(IContext<GameEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<GameEntity> context)
	{

	}
}
