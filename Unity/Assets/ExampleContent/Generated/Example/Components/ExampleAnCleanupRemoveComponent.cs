public partial class ExampleEntity
{
	static readonly ExampleContent.VisualDebugging.AnCleanupRemoveComponent AnCleanupRemoveComponent = new ExampleContent.VisualDebugging.AnCleanupRemoveComponent();

	public bool IsAnCleanupRemove
	{
		get { return HasComponent(ExampleComponentsLookup.AnCleanupRemove); }
		set
		{
			if (value != IsAnCleanupRemove)
			{
				var index = ExampleComponentsLookup.AnCleanupRemove;
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

public partial class ExampleEntity : IAnCleanupRemoveEntity { }

public sealed partial class ExampleMatcher
{
	static JCMG.EntitasRedux.IMatcher<ExampleEntity> _matcherAnCleanupRemove;

	public static JCMG.EntitasRedux.IMatcher<ExampleEntity> AnCleanupRemove
	{
		get
		{
			if (_matcherAnCleanupRemove == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ExampleEntity>)JCMG.EntitasRedux.Matcher<ExampleEntity>.AllOf(ExampleComponentsLookup.AnCleanupRemove);
				matcher.ComponentNames = ExampleComponentsLookup.ComponentNames;
				_matcherAnCleanupRemove = matcher;
			}

			return _matcherAnCleanupRemove;
		}
	}
}
