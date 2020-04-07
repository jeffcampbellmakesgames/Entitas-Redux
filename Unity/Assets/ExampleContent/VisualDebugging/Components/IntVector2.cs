using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	[ComponentName("PositionComponent", "VelocityComponent")]
	public struct IntVector2
	{
		public int x;
		public int y;
	}
}
