using JCMG.EntitasRedux;

public class MyTestCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public MyTestCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.MyTest;
		_cleanupSystems.Add(new DestroyMyTestEntitiesWithCleanupDestroySystem(context));
		_cleanupSystems.Add(new RemoveCleanupRemoveFromMyTestEntitiesSystem(context));
	}
	#endif

	public MyTestCleanupSystems(IContext<MyTestEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyMyTestEntitiesWithCleanupDestroySystem(context));
		_cleanupSystems.Add(new RemoveCleanupRemoveFromMyTestEntitiesSystem(context));
	}
}
