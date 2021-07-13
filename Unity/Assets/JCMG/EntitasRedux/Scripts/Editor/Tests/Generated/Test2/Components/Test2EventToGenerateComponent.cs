public partial class Test2Entity
{
	public EventToGenerateComponent EventToGenerate { get { return (EventToGenerateComponent)GetComponent(Test2ComponentsLookup.EventToGenerate); } }
	public bool HasEventToGenerate { get { return HasComponent(Test2ComponentsLookup.EventToGenerate); } }

	public void AddEventToGenerate(EntitasRedux.Tests.EventToGenerate newValue)
	{
		var index = Test2ComponentsLookup.EventToGenerate;
		var component = (EventToGenerateComponent)CreateComponent(index, typeof(EventToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceEventToGenerate(EntitasRedux.Tests.EventToGenerate newValue)
	{
		var index = Test2ComponentsLookup.EventToGenerate;
		var component = (EventToGenerateComponent)CreateComponent(index, typeof(EventToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyEventToGenerateTo(EventToGenerateComponent copyComponent)
	{
		var index = Test2ComponentsLookup.EventToGenerate;
		var component = (EventToGenerateComponent)CreateComponent(index, typeof(EventToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveEventToGenerate()
	{
		RemoveComponent(Test2ComponentsLookup.EventToGenerate);
	}
}

public partial class Test2Entity : IEventToGenerateEntity { }

public sealed partial class Test2Matcher
{
	static JCMG.EntitasRedux.IMatcher<Test2Entity> _matcherEventToGenerate;

	public static JCMG.EntitasRedux.IMatcher<Test2Entity> EventToGenerate
	{
		get
		{
			if (_matcherEventToGenerate == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<Test2Entity>)JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(Test2ComponentsLookup.EventToGenerate);
				matcher.ComponentNames = Test2ComponentsLookup.ComponentNames;
				_matcherEventToGenerate = matcher;
			}

			return _matcherEventToGenerate;
		}
	}
}
