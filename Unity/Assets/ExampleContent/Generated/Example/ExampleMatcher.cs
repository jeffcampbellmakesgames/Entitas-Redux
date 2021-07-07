public sealed partial class ExampleMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<ExampleEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<ExampleEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<ExampleEntity> AllOf(params JCMG.EntitasRedux.IMatcher<ExampleEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<ExampleEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<ExampleEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<ExampleEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<ExampleEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<ExampleEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<ExampleEntity>.AnyOf(matchers);
	}
}
