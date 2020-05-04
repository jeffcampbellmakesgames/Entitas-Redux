using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class SlowSystem : IExecuteSystem
	{
		public void Execute()
		{
			Thread.Sleep(4);
		}
	}
}
