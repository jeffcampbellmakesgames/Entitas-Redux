public partial class GameEntity
{
	static readonly EntitasRedux.Tests.CleanupEventComponent CleanupEventComponent = new EntitasRedux.Tests.CleanupEventComponent();

	public bool IsCleanupEvent
	{
		get { return HasComponent(GameComponentsLookup.CleanupEvent); }
		set
		{
			if (value != IsCleanupEvent)
			{
				var index = GameComponentsLookup.CleanupEvent;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: CleanupEventComponent;

					AddComponent(index, component);
				}
				else
				{
					RemoveComponent(index);
				}
			}
		}
	}
}

public sealed partial class GameMatcher
{
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherCleanupEvent;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> CleanupEvent
	{
		get
		{
			if (_matcherCleanupEvent == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.CleanupEvent);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherCleanupEvent = matcher;
			}

			return _matcherCleanupEvent;
		}
	}
}
