using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

namespace ExampleContent.VisualDebugging.CustomIndexes
{
	[CustomEntityIndex(typeof(VisualDebugContext))]
	public class ColorPositionEntityIndex : EntityIndex<VisualDebugEntity, IntVector2>
	{
		public ColorPositionEntityIndex(VisualDebugContext context) :
			base(
				nameof(ColorPositionEntityIndex),
				context.GetGroup(VisualDebugMatcher.AllOf(
					VisualDebugMatcher.Position,
					VisualDebugMatcher.Color)),
				(e, c) => e.Position.value)
		{

		}

		[EntityIndexGetMethod]
		public HashSet<VisualDebugEntity> GetColorEntitiesAtPosition(IntVector2 pos)
		{
			return GetEntities(pos);
		}
	}
}
