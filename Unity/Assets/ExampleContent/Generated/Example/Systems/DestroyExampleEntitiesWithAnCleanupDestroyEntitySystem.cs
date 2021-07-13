using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem : ICleanupSystem
{
	private readonly IGroup<ExampleEntity> _group;
	private readonly List<ExampleEntity> _entities;

	public DestroyExampleEntitiesWithAnCleanupDestroyEntitySystem(IContext<ExampleEntity> context)
	{
		_group = context.GetGroup(ExampleMatcher.AnCleanupDestroyEntity);
		_entities = new List<ExampleEntity>();
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
