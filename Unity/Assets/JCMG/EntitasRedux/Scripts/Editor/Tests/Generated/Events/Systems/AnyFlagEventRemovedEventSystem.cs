public sealed class AnyFlagEventRemovedEventSystem : JCMG.EntitasRedux.ReactiveSystem<TestEntity>
{
	readonly JCMG.EntitasRedux.IGroup<TestEntity> _listeners;
	readonly System.Collections.Generic.List<TestEntity> _entityBuffer;
	readonly System.Collections.Generic.List<IAnyFlagEventRemovedListener> _listenerBuffer;

	public AnyFlagEventRemovedEventSystem(Contexts contexts) : base(contexts.Test)
	{
		_listeners = contexts.Test.GetGroup(TestMatcher.AnyFlagEventRemovedListener);
		_entityBuffer = new System.Collections.Generic.List<TestEntity>();
		_listenerBuffer = new System.Collections.Generic.List<IAnyFlagEventRemovedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<TestEntity> GetTrigger(JCMG.EntitasRedux.IContext<TestEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Removed(TestMatcher.FlagEvent)
		);
	}

	protected override bool Filter(TestEntity entity)
	{
		return !entity.IsFlagEvent;
	}

	protected override void Execute(System.Collections.Generic.List<TestEntity> entities)
	{
		foreach (var e in entities)
		{
			
			foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
			{
				_listenerBuffer.Clear();
				_listenerBuffer.AddRange(listenerEntity.AnyFlagEventRemovedListener.value);
				foreach (var listener in _listenerBuffer)
				{
					listener.OnAnyFlagEventRemoved(e);
				}
			}
		}
	}
}
