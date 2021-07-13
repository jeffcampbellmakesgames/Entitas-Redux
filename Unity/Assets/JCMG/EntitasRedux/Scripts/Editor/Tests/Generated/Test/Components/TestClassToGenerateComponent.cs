public partial class TestEntity
{
	public ClassToGenerateComponent ClassToGenerate { get { return (ClassToGenerateComponent)GetComponent(TestComponentsLookup.ClassToGenerate); } }
	public bool HasClassToGenerate { get { return HasComponent(TestComponentsLookup.ClassToGenerate); } }

	public void AddClassToGenerate(EntitasRedux.Tests.ClassToGenerate newValue)
	{
		var index = TestComponentsLookup.ClassToGenerate;
		var component = (ClassToGenerateComponent)CreateComponent(index, typeof(ClassToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceClassToGenerate(EntitasRedux.Tests.ClassToGenerate newValue)
	{
		var index = TestComponentsLookup.ClassToGenerate;
		var component = (ClassToGenerateComponent)CreateComponent(index, typeof(ClassToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyClassToGenerateTo(ClassToGenerateComponent copyComponent)
	{
		var index = TestComponentsLookup.ClassToGenerate;
		var component = (ClassToGenerateComponent)CreateComponent(index, typeof(ClassToGenerateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveClassToGenerate()
	{
		RemoveComponent(TestComponentsLookup.ClassToGenerate);
	}
}

public partial class TestEntity : IClassToGenerateEntity { }

public sealed partial class TestMatcher
{
	static JCMG.EntitasRedux.IMatcher<TestEntity> _matcherClassToGenerate;

	public static JCMG.EntitasRedux.IMatcher<TestEntity> ClassToGenerate
	{
		get
		{
			if (_matcherClassToGenerate == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<TestEntity>)JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(TestComponentsLookup.ClassToGenerate);
				matcher.ComponentNames = TestComponentsLookup.ComponentNames;
				_matcherClassToGenerate = matcher;
			}

			return _matcherClassToGenerate;
		}
	}
}
