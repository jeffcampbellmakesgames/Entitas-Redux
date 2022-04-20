using JCMG.EntitasRedux;

public class ExampleCleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public ExampleCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Example);
	}
	#endif

	public ExampleCleanupFeature(IContext<ExampleEntity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<ExampleEntity> context)
	{
		Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
	}
}
