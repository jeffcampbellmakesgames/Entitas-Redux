using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public class TearDownSystem : ITearDownSystem
	{
		public void TearDown()
		{
			Debug.Log("TearDown");
		}
	}
}
