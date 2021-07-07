public sealed partial class VisualDebugMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<VisualDebugEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<VisualDebugEntity> AllOf(params JCMG.EntitasRedux.IMatcher<VisualDebugEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<VisualDebugEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<VisualDebugEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<VisualDebugEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<VisualDebugEntity>.AnyOf(matchers);
	}
}
