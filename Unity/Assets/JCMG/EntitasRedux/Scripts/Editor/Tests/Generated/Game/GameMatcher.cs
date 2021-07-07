public sealed partial class GameMatcher
{
	public static JCMG.EntitasRedux.IAllOfMatcher<GameEntity> AllOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(indices);
	}

	public static JCMG.EntitasRedux.IAllOfMatcher<GameEntity> AllOf(params JCMG.EntitasRedux.IMatcher<GameEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(matchers);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<GameEntity> AnyOf(params int[] indices)
	{
		return JCMG.EntitasRedux.Matcher<GameEntity>.AnyOf(indices);
	}

	public static JCMG.EntitasRedux.IAnyOfMatcher<GameEntity> AnyOf(params JCMG.EntitasRedux.IMatcher<GameEntity>[] matchers)
	{
		return JCMG.EntitasRedux.Matcher<GameEntity>.AnyOf(matchers);
	}
}
