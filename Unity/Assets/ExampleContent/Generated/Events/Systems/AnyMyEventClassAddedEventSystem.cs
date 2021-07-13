public sealed class AnyMyEventClassAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<VisualDebugEntity>
{
	readonly JCMG.EntitasRedux.IGroup<VisualDebugEntity> _listeners;
	readonly System.Collections.Generic.List<VisualDebugEntity> _entityBuffer;
	readonly System.Collections.Generic.List<IAnyMyEventClassAddedListener> _listenerBuffer;

	public AnyMyEventClassAddedEventSystem(Contexts contexts) : base(contexts.VisualDebug)
	{
		_listeners = contexts.VisualDebug.GetGroup(VisualDebugMatcher.AnyMyEventClassAddedListener);
		_entityBuffer = new System.Collections.Generic.List<VisualDebugEntity>();
		_listenerBuffer = new System.Collections.Generic.List<IAnyMyEventClassAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<VisualDebugEntity> GetTrigger(JCMG.EntitasRedux.IContext<VisualDebugEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(VisualDebugMatcher.MyEventClass)
		);
	}

	protected override bool Filter(VisualDebugEntity entity)
	{
		return entity.HasMyEventClass;
	}

	protected override void Execute(System.Collections.Generic.List<VisualDebugEntity> entities)
	{
		foreach (var e in entities)
		{
			var component = e.MyEventClass;
			foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
			{
				_listenerBuffer.Clear();
				_listenerBuffer.AddRange(listenerEntity.AnyMyEventClassAddedListener.value);
				foreach (var listener in _listenerBuffer)
				{
					listener.OnAnyMyEventClassAdded(e, component.value);
				}
			}
		}
	}
}
