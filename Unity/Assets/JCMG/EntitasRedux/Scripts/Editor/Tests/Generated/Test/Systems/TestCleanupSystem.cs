using JCMG.EntitasRedux;

public class TestCleanupSystems : JCMG.EntitasRedux.Systems
{
	public TestCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Test;
		_cleanupSystems.Add(new RemoveBaseFromTestEntitiesSystem(context));
	}

	public TestCleanupSystems(IContext<TestEntity> context) : base()
	{
		_cleanupSystems.Add(new RemoveBaseFromTestEntitiesSystem(context));
	}
}
