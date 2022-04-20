using JCMG.EntitasRedux;

public class VisualDebugCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public VisualDebugCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.VisualDebug;
		_cleanupSystems.Add(new DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromVisualDebugEntitiesSystem(context));
	}
	#endif

	public VisualDebugCleanupSystems(IContext<VisualDebugEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromVisualDebugEntitiesSystem(context));
	}
}
