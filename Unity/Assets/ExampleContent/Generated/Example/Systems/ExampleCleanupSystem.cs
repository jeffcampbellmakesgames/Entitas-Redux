//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v1.3.0, branch:develop).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using JCMG.EntitasRedux;

public class ExampleCleanupSystems : JCMG.EntitasRedux.Systems
{
	public ExampleCleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Example;
		_cleanupSystems.Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
		_cleanupSystems.Add(new DestroyExampleEntitiesWithEventGenBugSystem(context));
	}

	public ExampleCleanupSystems(IContext<ExampleEntity> context) : base()
	{
		_cleanupSystems.Add(new DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(context));
		_cleanupSystems.Add(new RemoveAnCleanupRemoveFromExampleEntitiesSystem(context));
		_cleanupSystems.Add(new DestroyExampleEntitiesWithEventGenBugSystem(context));
	}
}
