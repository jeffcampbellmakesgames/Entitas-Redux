using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class SomeReactiveSystem : ReactiveSystem<VisualDebugEntity>
	{
		public SomeReactiveSystem(Contexts contexts) : base(contexts.VisualDebug)
		{
		}

		protected override ICollector<VisualDebugEntity> GetTrigger(IContext<VisualDebugEntity> context)
		{
			return context.CreateCollector(Matcher<VisualDebugEntity>.AllOf(0));
		}

		protected override bool Filter(VisualDebugEntity entity)
		{
			return true;
		}

		protected override void Execute(List<VisualDebugEntity> entities)
		{
		}
	}
}
