using JCMG.EntitasRedux;

public class ExampleCleanupFeature : Feature
{
	public ExampleCleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Example);
	}

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
