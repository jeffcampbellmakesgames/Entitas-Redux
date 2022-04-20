using JCMG.EntitasRedux;

public class TestCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public TestCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Test);
	}
	#endif

	public TestCleanupFeature(IContext<TestEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<TestEntity> context)
	{
		Add(new RemoveBaseFromTestEntitiesSystem(context));
	}
}
