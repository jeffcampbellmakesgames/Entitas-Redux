using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class SlowSystem : IUpdateSystem
	{
		public void Update()
		{
			Thread.Sleep(4);
		}
	}
}
