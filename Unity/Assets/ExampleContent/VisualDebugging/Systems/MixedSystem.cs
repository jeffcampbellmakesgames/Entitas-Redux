using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class MixedSystem : IInitializeSystem,
	                           IUpdateSystem,
	                           ICleanupSystem,
	                           ITearDownSystem
	{
		public void Cleanup()
		{
			//UnityEngine.Debug.Log("Cleanup");
		}

		public void Update()
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
