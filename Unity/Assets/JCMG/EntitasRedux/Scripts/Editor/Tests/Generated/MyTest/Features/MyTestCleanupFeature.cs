using JCMG.EntitasRedux;

public class MyTestCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public MyTestCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.MyTest);
	}
	#endif

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
