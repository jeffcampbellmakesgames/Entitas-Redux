namespace JCMG.EntitasRedux.Editor.Plugins
{
	/// <summary>
	/// An exception thrown when two or more components belonging to the same Context have the same name. This would
	/// result in an error during code-generation.
	/// </summary>
	public class DuplicateComponentNameException : EntitasReduxException
	{
		private const string DUPLICATE_COMPONENT_NAME_ERROR = "Two or more components belonging to the same Context " +
		                                                      "have the same name.  Please ensure all components " +
		                                                      "belonging to a single context have unique names.";

		public DuplicateComponentNameException() : base(DUPLICATE_COMPONENT_NAME_ERROR, string.Empty)
		{

		}
	}
}
