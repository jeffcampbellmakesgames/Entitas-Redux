using JCMG.EntitasRedux;

public class ExampleCleanupSystems : JCMG.EntitasRedux.Systems
{
	public ExampleCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Example;
		_cleanupSystems.Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
	}

	public ExampleCleanupSystems(IContext<ExampleEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
	}
}
