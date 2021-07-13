using JCMG.EntitasRedux;

public class TestCleanupFeature : Feature
{
	public TestCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Test);
	}

	public TestCleanupFeature(IContext<TestEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<TestEntity> context)
	{
		Add(new RemoveBaseFromTestEntitiesSystem(context));
	}
}
