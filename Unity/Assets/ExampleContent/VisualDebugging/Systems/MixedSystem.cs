using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class MixedSystem : IInitializeSystem,
	                           IExecuteSystem,
	                           ICleanupSystem,
	                           ITearDownSystem
	{
		public void Cleanup()
		{
			//UnityEngine.Debug.Log("Cleanup");
		}

		public void Execute()
		{
			//UnityEngine.Debug.Log("Execute");
		}

		public void Initialize()
		{
			//UnityEngine.Debug.Log("Initialize");
		}

		public void TearDown()
		{
			//UnityEngine.Debug.Log("TearDown");
		}
	}
}
