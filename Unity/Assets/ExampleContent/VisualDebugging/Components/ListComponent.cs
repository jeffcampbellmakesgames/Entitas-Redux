using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class ListComponent : IComponent
	{
		public List<string> list;
	}
}
