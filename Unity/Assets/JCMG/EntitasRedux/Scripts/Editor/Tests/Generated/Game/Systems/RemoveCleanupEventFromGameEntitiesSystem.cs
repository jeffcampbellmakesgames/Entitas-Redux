using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class RemoveCleanupEventFromGameEntitiesSystem : ICleanupSystem
{
	private readonly IGroup<GameEntity> _group;
	private readonly List<GameEntity> _entities;

	public RemoveCleanupEventFromGameEntitiesSystem(IContext<GameEntity> context)
	{
		_group = context.GetGroup(GameMatcher.CleanupEvent);
		_entities = new List<GameEntity>();
	}

	/// <summary>
	/// Performs cleanup logic after other systems have executed.
	/// </summary>
	public void Cleanup()
	{
		_group.GetEntities(_entities);
		for (var i = 0; i < _entities.Count; ++i)
		{
			_entities[i].IsCleanupEvent = false;
		}
	}
}
