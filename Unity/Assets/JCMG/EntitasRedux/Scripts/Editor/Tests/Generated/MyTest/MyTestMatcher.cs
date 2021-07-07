public sealed partial class MyTestMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<MyTestEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<MyTestEntity> AllOf(params JCMG.EntitasRedux.IMatcher<MyTestEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<MyTestEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<MyTestEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<MyTestEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<MyTestEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<MyTestEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<MyTestEntity>.AnyOf(matchers);
	}
}
