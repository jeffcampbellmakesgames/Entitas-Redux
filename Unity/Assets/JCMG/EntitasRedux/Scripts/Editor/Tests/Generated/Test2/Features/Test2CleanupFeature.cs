using JCMG.EntitasRedux;

public class Test2CleanupFeature : Feature
{
	public Test2CleanupFeature() : base()
	{
		AddSystems(Contexts.SharedInstance.Test2);
	}

	public Test2CleanupFeature(IContext<Test2Entity> context) : base()
	{
		AddSystems(context);
	}

	private void AddSystems(IContext<Test2Entity> context)
	{

	}
}
