using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class RemoveBaseFromTestEntitiesSystem : ICleanupSystem
{
	private readonly IGroup<TestEntity> _group;
	private readonly List<TestEntity> _entities;

	public RemoveBaseFromTestEntitiesSystem(IContext<TestEntity> context)
	{
		_group = context.GetGroup(TestMatcher.Base);
		_entities = new List<TestEntity>();
	}

	/// <summary>
	/// Performs cleanup logic after other systems have executed.
	/// </summary>
	public void Cleanup()
	{
		_group.GetEntities(_entities);
		for (var i = 0; i < _entities.Count; ++i)
		{
			_entities[i].RemoveBase();
		}
	}
}
