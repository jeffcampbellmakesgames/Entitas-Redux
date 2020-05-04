using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	public class ProcessRandomValueSystem : ReactiveSystem<VisualDebugEntity>
	{
		public ProcessRandomValueSystem(Contexts contexts) : base(contexts.VisualDebug)
		{
		}

		protected override ICollector<VisualDebugEntity> GetTrigger(IContext<VisualDebugEntity> context)
		{
			return context.CreateCollector(VisualDebugMatcher.MyFloat);
		}

		protected override bool Filter(VisualDebugEntity entity)
		{
			return true;
		}

		protected override void Execute(List<VisualDebugEntity> entities)
		{
			foreach (var e in entities)
			{
				e.Destroy();
			}
		}
	}
}
