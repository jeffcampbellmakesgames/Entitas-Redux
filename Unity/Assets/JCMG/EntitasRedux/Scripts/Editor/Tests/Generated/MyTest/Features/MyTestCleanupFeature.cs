using JCMG.EntitasRedux;

public class MyTestCleanupFeature : Feature
{
	public MyTestCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.MyTest);
	}

	public MyTestCleanupFeature(IContext<MyTestEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<MyTestEntity> context)
	{
		Add(new DestroyMyTestEntitiesWithCleanupDestroySystem(context));
		Add(new RemoveCleanupRemoveFromMyTestEntitiesSystem(context));
	}
}
