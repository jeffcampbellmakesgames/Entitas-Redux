using JCMG.EntitasRedux;

public class VisualDebugCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public VisualDebugCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.VisualDebug);
	}
	#endif

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
