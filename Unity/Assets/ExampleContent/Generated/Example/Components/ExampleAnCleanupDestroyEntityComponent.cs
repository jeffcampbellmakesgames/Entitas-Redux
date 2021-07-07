public partial class ExampleEntity
{
	static readonly ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent AnCleanupDestroyEntityComponent = new ExampleContent.VisualDebugging.AnCleanupDestroyEntityComponent();

	public bool IsAnCleanupDestroyEntity
	{
		get { return HasComponent(ExampleComponentsLookup.AnCleanupDestroyEntity); }
		set
		{
			if (value != IsAnCleanupDestroyEntity)
			{
				var index = ExampleComponentsLookup.AnCleanupDestroyEntity;
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

public partial class ExampleEntity : IAnCleanupDestroyEntityEntity { }

public sealed partial class ExampleMatcher
{
	static JCMG.EntitasRedux.IMatcher<ExampleEntity> _matcherAnCleanupDestroyEntity;

	public static JCMG.EntitasRedux.IMatcher<ExampleEntity> AnCleanupDestroyEntity
	{
		get
		{
			if (_matcherAnCleanupDestroyEntity == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ExampleEntity>)JCMG.EntitasRedux.Matcher<ExampleEntity>.AllOf(ExampleComponentsLookup.AnCleanupDestroyEntity);
				matcher.ComponentNames = ExampleComponentsLookup.ComponentNames;
				_matcherAnCleanupDestroyEntity = matcher;
			}

			return _matcherAnCleanupDestroyEntity;
		}
	}
}
