using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem : ICleanupSystem
{
	private readonly IGroup<VisualDebugEntity> _group;
	private readonly List<VisualDebugEntity> _entities;

	public DestroyVisualDebugEntitiesWithAnCleanupDestroyEntitySystem(IContext<VisualDebugEntity> context)
	{
		_group = context.GetGroup(VisualDebugMatcher.AnCleanupDestroyEntity);
		_entities = new List<VisualDebugEntity>();
	}

	/// <summary>
	/// Performs cleanup logic after other systems have executed.
	/// </summary>
	public void Cleanup()
	{
		_group.GetEntities(_entities);
		for (var i = 0; i < _entities.Count; ++i)
		{
			_entities[i].Destroy();
		}
	}
}
