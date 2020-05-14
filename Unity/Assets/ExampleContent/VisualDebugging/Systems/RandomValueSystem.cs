using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public class RandomValueSystem : IUpdateSystem
	{
		private readonly VisualDebugContext _context;

		public RandomValueSystem(Contexts contexts)
		{
			_context = contexts.VisualDebug;
		}

		public void Update()
		{
			_context.CreateEntity().AddMyFloat(Random.value);
		}
	}
}
