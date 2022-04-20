using JCMG.EntitasRedux;

public class ExampleCleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public ExampleCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Example;
		_cleanupSystems.Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
	}
	#endif

	public ExampleCleanupSystems(IContext<ExampleEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
	}
}
