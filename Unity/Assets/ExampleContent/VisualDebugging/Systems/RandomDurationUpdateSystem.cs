using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public sealed class RandomDurationUpdateSystem : RandomDurationSystem,
											  IUpdateSystem
	{
		/// <summary>
		/// Executes logic per render frame.
		/// </summary>
		public void Update()
		{
			SleepRandomDuration();
		}
	}
}
