using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class ListArrayComponent : IComponent
	{
		public List<string>[] listArray;
	}
}
