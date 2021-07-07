public sealed class TestAnyMultipleEventsStandardEventAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<TestEntity>
{
	readonly JCMG.EntitasRedux.IGroup<TestEntity> _listeners;
	readonly System.Collections.Generic.List<TestEntity> _entityBuffer;
	readonly System.Collections.Generic.List<ITestAnyMultipleEventsStandardEventAddedListener> _listenerBuffer;

	public TestAnyMultipleEventsStandardEventAddedEventSystem(Contexts contexts) : base(contexts.Test)
	{
		_listeners = contexts.Test.GetGroup(TestMatcher.TestAnyMultipleEventsStandardEventAddedListener);
		_entityBuffer = new System.Collections.Generic.List<TestEntity>();
		_listenerBuffer = new System.Collections.Generic.List<ITestAnyMultipleEventsStandardEventAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<TestEntity> GetTrigger(JCMG.EntitasRedux.IContext<TestEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(TestMatcher.MultipleEventsStandardEvent)
		);
	}

	protected override bool Filter(TestEntity entity)
	{
		return entity.HasMultipleEventsStandardEvent;
	}

	protected override void Execute(System.Collections.Generic.List<TestEntity> entities)
	{
		foreach (var e in entities)
		{
			var component = e.MultipleEventsStandardEvent;
			foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
			{
				_listenerBuffer.Clear();
				_listenerBuffer.AddRange(listenerEntity.TestAnyMultipleEventsStandardEventAddedListener.value);
				foreach (var listener in _listenerBuffer)
				{
					listener.OnAnyMultipleEventsStandardEventAdded(e, component.value);
				}
			}
		}
	}
}
