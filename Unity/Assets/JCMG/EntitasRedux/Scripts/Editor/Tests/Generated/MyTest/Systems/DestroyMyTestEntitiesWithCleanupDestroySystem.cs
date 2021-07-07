using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class DestroyMyTestEntitiesWithCleanupDestroySystem : ICleanupSystem
{
	private readonly IGroup<MyTestEntity> _group;
	private readonly List<MyTestEntity> _entities;

	public DestroyMyTestEntitiesWithCleanupDestroySystem(IContext<MyTestEntity> context)
	{
		_group = context.GetGroup(MyTestMatcher.CleanupDestroy);
		_entities = new List<MyTestEntity>();
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
