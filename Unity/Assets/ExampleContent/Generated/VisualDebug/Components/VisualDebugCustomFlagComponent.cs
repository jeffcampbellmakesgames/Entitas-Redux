public partial class VisualDebugEntity
{
	static readonly ExampleContent.VisualDebugging.CustomFlagComponent CustomFlagComponent = new ExampleContent.VisualDebugging.CustomFlagComponent();

	public bool MyCustomFlag
	{
		get { return HasComponent(VisualDebugComponentsLookup.CustomFlag); }
		set
		{
			if (value != MyCustomFlag)
			{
				var index = VisualDebugComponentsLookup.CustomFlag;
				if (value)
				{
					var componentPool = GetComponentPool(index);
					var component = componentPool.Count > 0
							? componentPool.Pop()
							: CustomFlagComponent;

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

public sealed partial class VisualDebugMatcher
{
	static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> _matcherCustomFlag;

	public static JCMG.EntitasRedux.IMatcher<VisualDebugEntity> CustomFlag
	{
		get
		{
			if (_matcherCustomFlag == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<VisualDebugEntity>)JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(VisualDebugComponentsLookup.CustomFlag);
				matcher.ComponentNames = VisualDebugComponentsLookup.ComponentNames;
				_matcherCustomFlag = matcher;
			}

			return _matcherCustomFlag;
		}
	}
}
