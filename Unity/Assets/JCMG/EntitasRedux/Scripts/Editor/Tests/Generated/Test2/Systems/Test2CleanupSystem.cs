using JCMG.EntitasRedux;

public class Test2CleanupSystems : JCMG.EntitasRedux.Systems
{
	public Test2CleanupSystems() : base()
	{
		var context = Contexts.SharedInstance.Test2;

	}

	public Test2CleanupSystems(IContext<Test2Entity> context) : base()
	{

	}
}
