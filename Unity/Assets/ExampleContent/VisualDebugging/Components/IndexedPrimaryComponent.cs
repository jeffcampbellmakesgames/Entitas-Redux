﻿using System;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging
{
	[Serializable]
	[VisualDebug]
	public class IndexedPrimaryComponent : IComponent
	{
		[PrimaryEntityIndex]
		public int id;
	}
}
