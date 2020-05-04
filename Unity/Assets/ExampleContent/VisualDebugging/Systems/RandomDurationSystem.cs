using System.Threading;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public class RandomDurationSystem : IExecuteSystem
	{
		public void Execute()
		{
			Thread.Sleep(Random.Range(0, 9));
		}
	}
}
