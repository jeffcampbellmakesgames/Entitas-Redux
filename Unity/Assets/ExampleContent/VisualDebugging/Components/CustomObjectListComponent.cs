using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class CustomObjectListComponent : IComponent
	{
		public List<CustomObject> value;
	}
}
