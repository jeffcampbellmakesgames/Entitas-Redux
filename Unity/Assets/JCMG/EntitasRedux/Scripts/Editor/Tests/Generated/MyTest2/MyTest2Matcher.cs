public sealed partial class MyTest2Matcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<MyTest2Entity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<MyTest2Entity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<MyTest2Entity> AllOf(params JCMG.EntitasRedux.IMatcher<MyTest2Entity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<MyTest2Entity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<MyTest2Entity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<MyTest2Entity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<MyTest2Entity> AnyOf(params JCMG.EntitasRedux.IMatcher<MyTest2Entity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<MyTest2Entity>.AnyOf(matchers);
	}
}
