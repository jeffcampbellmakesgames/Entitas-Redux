using JCMG.EntitasRedux;

public class MyTestCleanupSystems : JCMG.EntitasRedux.Systems
{
	public MyTestCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.MyTest;
		_cleanupSystems.Add(new DestroyMyTestEntitiesWithCleanupDestroySystem(context));
		_cleanupSystems.Add(new RemoveCleanupRemoveFromMyTestEntitiesSystem(context));
	}

	public MyTestCleanupSystems(IContext<MyTestEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyMyTestEntitiesWithCleanupDestroySystem(context));
		_cleanupSystems.Add(new RemoveCleanupRemoveFromMyTestEntitiesSystem(context));
	}
}
