using JCMG.EntitasRedux;

public class GameCleanupFeature : Feature
{
	public GameCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Game);
	}

	public GameCleanupFeature(IContext<GameEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<GameEntity> context)
	{

	}
}
