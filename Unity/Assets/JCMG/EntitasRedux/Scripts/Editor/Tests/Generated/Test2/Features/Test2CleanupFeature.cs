using JCMG.EntitasRedux;

public class Test2CleanupFeature : Feature
{
	#if !ENTITAS_REDUX_NO_SHARED_CONTEXT
	public Test2CleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Test2);
	}
	#endif

	public Test2CleanupFeature(IContext<Test2Entity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<Test2Entity> context)
	{

	}
}
