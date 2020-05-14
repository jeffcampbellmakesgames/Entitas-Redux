using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class SlowInitializeUpdateSystem : IInitializeSystem,
	                                           IUpdateSystem
	{
		public void Update()
		{
			Thread.Sleep(5);
		}

		public void Initialize()
		{
			Thread.Sleep(10);
		}
	}
}
