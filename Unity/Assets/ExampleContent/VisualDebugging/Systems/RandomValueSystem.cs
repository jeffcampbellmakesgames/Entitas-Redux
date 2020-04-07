using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public class RandomValueSystem : IExecuteSystem
	{
		private readonly VisualDebugContext _context;

		public RandomValueSystem(Contexts contexts)
		{
			_context = contexts.VisualDebug;
		}

		public void Execute()
		{
			_context.CreateEntity().AddMyFloat(Random.value);
		}
	}
}
