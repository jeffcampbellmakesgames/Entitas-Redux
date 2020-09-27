namespace JCMG.EntitasRedux
{
	/// <summary>
	/// An exception that occurs when an index is attempted to be used with a component lookup that is out of range for
	/// the components it contains.
	/// </summary>
	public sealed class IndexOutOfLookupRangeException : EntitasReduxException
	{
		public IndexOutOfLookupRangeException(string message, string hint) : base(message, hint)
		{
		}
	}
}
