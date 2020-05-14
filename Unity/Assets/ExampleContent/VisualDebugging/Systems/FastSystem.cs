using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class FastSystem : IFixedUpdateSystem
	{
		/// <summary>
		/// Executes physics logic.
		/// </summary>
		public void FixedUpdate()
		{
			Thread.Sleep(2);
		}
	}
}
