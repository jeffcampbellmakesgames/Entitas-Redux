public sealed class CleanupEventAddedEventSystem : JCMG.EntitasRedux.ReactiveSystem<GameEntity>
{
	readonly System.Collections.Generic.List<ICleanupEventAddedListener> _listenerBuffer;

	public CleanupEventAddedEventSystem(Contexts contexts) : base(contexts.Game)
	{
		_listenerBuffer = new System.Collections.Generic.List<ICleanupEventAddedListener>();
	}

	protected override JCMG.EntitasRedux.ICollector<GameEntity> GetTrigger(JCMG.EntitasRedux.IContext<GameEntity> context)
	{
		return JCMG.EntitasRedux.CollectorContextExtension.CreateCollector(
			context,
			JCMG.EntitasRedux.TriggerOnEventMatcherExtension.Added(GameMatcher.CleanupEvent)
		);
	}

	protected override bool Filter(GameEntity entity)
	{
		return entity.IsCleanupEvent && entity.HasCleanupEventAddedListener;
	}

	protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
	{
		foreach (var e in entities)
		{
			
			_listenerBuffer.Clear();
			_listenerBuffer.AddRange(e.CleanupEventAddedListener.value);
			foreach (var listener in _listenerBuffer)
			{
				listener.OnCleanupEventAdded(e);
			}
		}
	}
}
