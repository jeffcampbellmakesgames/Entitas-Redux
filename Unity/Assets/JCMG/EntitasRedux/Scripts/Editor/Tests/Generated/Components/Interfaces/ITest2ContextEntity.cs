public partial interface ITest2ContextEntity
{
	EntitasRedux.Tests.Test2ContextComponent Test2Context { get; }
	bool HasTest2Context { get; }

	void AddTest2Context(string newValue);
	void ReplaceTest2Context(string newValue);
	void RemoveTest2Context();
}
