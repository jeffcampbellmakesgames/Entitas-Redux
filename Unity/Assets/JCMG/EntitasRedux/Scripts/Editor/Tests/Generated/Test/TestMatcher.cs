public sealed partial class TestMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<TestEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<TestEntity> AllOf(params JCMG.EntitasRedux.IMatcher<TestEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<TestEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<TestEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<TestEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<TestEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<TestEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<TestEntity>.AnyOf(matchers);
	}
}
