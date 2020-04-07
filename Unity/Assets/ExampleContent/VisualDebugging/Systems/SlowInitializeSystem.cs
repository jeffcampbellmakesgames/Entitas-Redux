using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class SlowInitializeSystem : IInitializeSystem
	{
		public void Initialize()
		{
			Thread.Sleep(30);
		}
	}
}
