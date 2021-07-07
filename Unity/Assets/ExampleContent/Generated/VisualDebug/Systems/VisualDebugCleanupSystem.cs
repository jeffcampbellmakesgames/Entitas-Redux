using JCMG.EntitasRedux;

public class VisualDebugCleanupSystems : JCMG.EntitasRedux.Systems
{
	public VisualDebugCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.VisualDebug;
		_cleanupSystems.Add(new DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromVisualDebugEntitiesSystem(context));
	}

	public VisualDebugCleanupSystems(IContext<VisualDebugEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromVisualDebugEntitiesSystem(context));
	}
}
