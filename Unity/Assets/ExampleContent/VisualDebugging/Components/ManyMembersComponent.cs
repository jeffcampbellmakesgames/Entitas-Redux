using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class ManyMembersComponent : IComponent
	{
		public string field1;
		public string field10;
		public string field11;
		public string field12;
		public string field2;
		public string field3;
		public string field4;
		public string field5;
		public string field6;
		public string field7;
		public string field8;
		public string field9;
	}
}
