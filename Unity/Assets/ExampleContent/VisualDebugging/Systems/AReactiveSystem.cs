using System.Collections.Generic;
using System.Threading;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class AReactiveSystem : ReactiveSystem<VisualDebugEntity>
	{
		public AReactiveSystem(Contexts contexts) : base(contexts.VisualDebug)
		{
		}

		protected override ICollector<VisualDebugEntity> GetTrigger(IContext<VisualDebugEntity> context)
		{
			return context.CreateCollector(VisualDebugMatcher.MyString);
		}

		protected override bool Filter(VisualDebugEntity entity)
		{
			return true;
		}

		protected override void Execute(List<VisualDebugEntity> entities)
		{
			Thread.Sleep(2);
		}
	}
}
