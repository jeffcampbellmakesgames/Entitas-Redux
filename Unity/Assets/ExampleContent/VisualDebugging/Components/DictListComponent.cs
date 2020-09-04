using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[VisualDebug]
	public class DictListComponent : IComponent
	{
		public Dictionary<int, List<CustomObject>> dictRefListType;
		public Dictionary<int, List<List<CustomObject>>> dictRefNestedListType;
	}
}
