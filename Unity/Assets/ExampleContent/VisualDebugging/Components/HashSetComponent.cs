using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class HashSetComponent : IComponent
	{
		public HashSet<string> hashset;
	}
}
