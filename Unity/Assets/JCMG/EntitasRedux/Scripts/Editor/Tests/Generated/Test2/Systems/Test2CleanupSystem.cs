using JCMG.EntitasRedux;

public class Test2CleanupSystems : JCMG.EntitasRedux.Systems
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public Test2CleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Test2;

	}
	#endif

	public Test2CleanupSystems(IContext<Test2Entity> context) : base()
	{

	}
}
