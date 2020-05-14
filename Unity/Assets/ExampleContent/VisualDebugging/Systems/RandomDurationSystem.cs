using System.Threading;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public abstract class RandomDurationSystem
	{
		public void SleepRandomDuration()
		{
			Thread.Sleep(Random.Range(0, 5));
		}
	}
}
