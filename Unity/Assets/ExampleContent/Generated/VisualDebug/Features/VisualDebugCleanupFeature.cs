using JCMG.EntitasRedux;

public class VisualDebugCleanupFeature : Feature
{
	public VisualDebugCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.VisualDebug);
	}

	public VisualDebugCleanupFeature(IContext<VisualDebugEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<VisualDebugEntity> context)
	{
		Add(new DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem(context));
		Add(new RemoveAnCleanupRemoveFromVisualDebugEntitiesSystem(context));
	}
}
