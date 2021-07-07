public sealed class StandardEntityEventRemovedEventSystem : JCMG.EntitasRedux.ReactiveSystem<TestEntity>
{
	readonly System.Collections.Generic.List<IStandardEntityEventRemovedListener> _listenerBuffer;

	public StandardEntityEventRemovedEventSystem(Contexts contexts) : base(contexts.Test)
	{
		_listenerBuffer = new System.Collections.Generic.List<IStandardEntityEventRemovedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<TestEntity> GetTrigger(JCMG.EntitasRedux.IContext<TestEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Removed(TestMatcher.StandardEntityEvent)
		);
	}

	protected override bool Filter(TestEntity entity)
	{
		return !entity.HasStandardEntityEvent && entity.HasStandardEntityEventRemovedListener;
	}

	protected override void Execute(System.Collections.Generic.List<TestEntity> entities)
	{
		foreach (var e in entities)
		{
			
			_listenerBuffer.Clear();
			_listenerBuffer.AddRange(e.StandardEntityEventRemovedListener.value);
			foreach (var listener in _listenerBuffer)
			{
				listener.OnStandardEntityEventRemoved(e);
			}
		}
	}
}
