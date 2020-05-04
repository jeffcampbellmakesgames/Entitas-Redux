using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class SlowInitializeExecuteSystem : IInitializeSystem,
	                                           IExecuteSystem
	{
		public void Execute()
		{
			Thread.Sleep(5);
		}

		public void Initialize()
		{
			Thread.Sleep(10);
		}
	}
}
