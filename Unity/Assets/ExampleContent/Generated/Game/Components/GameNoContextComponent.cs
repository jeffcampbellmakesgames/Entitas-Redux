public partial class GameEntity
{
	static readonly ExampleContent.VisualDebugging.NoContextComponent NoContextComponent = new ExampleContent.VisualDebugging.NoContextComponent();

	public bool IsNoContext
	{
		get { return HasComponent(GameComponentsLookup.NoContext); }
		set
		{
			if (value != IsNoContext)
			{
				var index = GameComponentsLookup.NoContext;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: NoContextComponent;

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
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherNoContext;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> NoContext
	{
		get
		{
			if (_matcherNoContext == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.NoContext);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherNoContext = matcher;
			}

			return _matcherNoContext;
		}
	}
}
