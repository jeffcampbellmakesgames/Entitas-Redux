public partial class VisualDebugEntity
{
	static readonly ExampleContent.VisualDebugging.AnCleanupRemoveComponent AnCleanupRemoveComponent = new ExampleContent.VisualDebugging.AnCleanupRemoveComponent();

	public bool IsAnCleanupRemove
	{
		get { return HasComponent(VisualDebugComponentsLookup.AnCleanupRemove); }
		set
		{
			if (value != IsAnCleanupRemove)
			{
				var index = VisualDebugComponentsLookup.AnCleanupRemove;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: AnCleanupRemoveComponent;

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

public partial class VisualDebugEntity : IAnCleanupRemoveEntity { }

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherAnCleanupRemove;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> AnCleanupRemove
	{
		get
		{
			if (_matcherAnCleanupRemove == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.AnCleanupRemove);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherAnCleanupRemove = matcher;
			}

			return _matcherAnCleanupRemove;
		}
	}
}
