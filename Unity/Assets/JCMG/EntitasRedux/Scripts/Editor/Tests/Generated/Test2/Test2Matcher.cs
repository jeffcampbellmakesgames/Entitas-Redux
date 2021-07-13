public sealed partial class Test2Matcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<Test2Entity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<Test2Entity> AllOf(params JCMG.EntitasRedux.IMatcher<Test2Entity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<Test2Entity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<Test2Entity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<Test2Entity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<Test2Entity> AnyOf(params JCMG.EntitasRedux.IMatcher<Test2Entity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<Test2Entity>.AnyOf(matchers);
	}
}
