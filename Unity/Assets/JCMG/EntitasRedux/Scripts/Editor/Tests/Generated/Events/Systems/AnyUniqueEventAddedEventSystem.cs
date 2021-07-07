public sealed class AnyUniqueEventAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<TestEntity>
{
	readonly JCMG.EntitasRedux.IGroup<TestEntity> _listeners;
	readonly System.Collections.Generic.List<TestEntity> _entityBuffer;
	readonly System.Collections.Generic.List<IAnyUniqueEventAddedListener> _listenerBuffer;

	public AnyUniqueEventAddedEventSystem(Contexts contexts) : base(contexts.Test)
	{
		_listeners = contexts.Test.GetGroup(TestMatcher.AnyUniqueEventAddedListener);
		_entityBuffer = new System.Collections.Generic.List<TestEntity>();
		_listenerBuffer = new System.Collections.Generic.List<IAnyUniqueEventAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<TestEntity> GetTrigger(JCMG.EntitasRedux.IContext<TestEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(TestMatcher.UniqueEvent)
		);
	}

	protected override bool Filter(TestEntity entity)
	{
		return entity.HasUniqueEvent;
	}

	protected override void Execute(System.Collections.Generic.List<TestEntity> entities)
	{
		foreach (var e in entities)
		{
			var component = e.UniqueEvent;
			foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
			{
				_listenerBuffer.Clear();
				_listenerBuffer.AddRange(listenerEntity.AnyUniqueEventAddedListener.value);
				foreach (var listener in _listenerBuffer)
				{
					listener.OnAnyUniqueEventAdded(e, component.value);
				}
			}
		}
	}
}
