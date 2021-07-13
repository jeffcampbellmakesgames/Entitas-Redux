using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class IndexedEntityComponent : IComponent
	{
		[EntityIndex]
		public int id;
	}
}
