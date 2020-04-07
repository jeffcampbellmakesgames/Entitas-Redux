using System.Collections.Generic;
using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public class SomeMultiReactiveSystem : MultiReactiveSystem<VisualDebugEntity, Contexts>
	{
		public SomeMultiReactiveSystem(Contexts contexts) : base(contexts)
		{
		}

		protected override ICollector[] GetTrigger(Contexts contexts)
		{
			return new ICollector[]
			{
				contexts.VisualDebug.CreateCollector(VisualDebugMatcher.MyString)
			};
		}

		protected override bool Filter(VisualDebugEntity entity)
		{
			return true;
		}

		protected override void Execute(List<VisualDebugEntity> entities)
		{
			foreach (var e in entities)
			{
				// Methods are available
				var str = e.MyString;

				Debug.Log("Processed: " + e);
			}
		}
	}
}
