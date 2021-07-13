public partial class VisualDebugEntity
{
	static readonly ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent AnCleanupDestroyEntityComponent = new ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent();

	public bool IsAnCleanupDestroyEntity
	{
		get { return HasComponent(VisualDebugComponentsLookup.AnCleanupDestroyEntity); }
		set
		{
			if (value != IsAnCleanupDestroyEntity)
			{
				var index = VisualDebugComponentsLookup.AnCleanupDestroyEntity;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: AnCleanupDestroyEntityComponent;

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

public partial class VisualDebugEntity : IAnCleanupDestroyEntityEntity { }

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherAnCleanupDestroyEntity;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> AnCleanupDestroyEntity
	{
		get
		{
			if (_matcherAnCleanupDestroyEntity == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.AnCleanupDestroyEntity);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherAnCleanupDestroyEntity = matcher;
			}

			return _matcherAnCleanupDestroyEntity;
		}
	}
}
