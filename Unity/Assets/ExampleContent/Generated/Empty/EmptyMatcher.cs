public sealed partial class EmptyMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<EmptyEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<EmptyEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<EmptyEntity> AllOf(params JCMG.EntitasRedux.IMatcher<EmptyEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<EmptyEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<EmptyEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<EmptyEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<EmptyEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<EmptyEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<EmptyEntity>.AnyOf(matchers);
	}
}
