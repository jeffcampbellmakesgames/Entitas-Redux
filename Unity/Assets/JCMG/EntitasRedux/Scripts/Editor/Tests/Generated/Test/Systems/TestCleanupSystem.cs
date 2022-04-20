using JCMG.EntitasRedux;

public class TestCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public TestCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Test;
		_cleanupSystems.Add(new RemoveBaseFromTestEntitiesSystem(context));
	}
	#endif

	public TestCleanupSystems(IContext<TestEntity> context) : base()
	{
		_cleanupSystems.Add(new RemoveBaseFromTestEntitiesSystem(context));
	}
}
