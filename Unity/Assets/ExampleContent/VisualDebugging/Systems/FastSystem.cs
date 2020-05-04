using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class FastSystem : IExecuteSystem
	{
		public void Execute()
		{
			Thread.Sleep(1);
		}
	}
}
