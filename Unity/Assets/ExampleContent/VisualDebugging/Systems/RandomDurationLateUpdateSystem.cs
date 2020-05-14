using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public sealed class RandomDurationLateUpdateSystem : RandomDurationSystem,
														 ILateUpdateSystem
	{
		/// <summary>
		/// Executes logic at the end of the frame render.
		/// </summary>
		public void LateUpdate()
		{
			SleepRandomDuration();
		}
	}
}
