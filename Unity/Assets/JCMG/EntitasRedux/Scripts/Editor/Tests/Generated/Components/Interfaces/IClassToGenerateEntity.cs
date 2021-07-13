public partial interface IClassToGenerateEntity
{
	ClassToGenerateComponent ClassToGenerate { get; }
	bool HasClassToGenerate { get; }

	void AddClassToGenerate(EntitasRedux.Tests.ClassToGenerate newValue);
	void ReplaceClassToGenerate(EntitasRedux.Tests.ClassToGenerate newValue);
	void RemoveClassToGenerate();
}
