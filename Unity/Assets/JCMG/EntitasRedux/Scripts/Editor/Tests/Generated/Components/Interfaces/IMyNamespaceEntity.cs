public partial interface IMyNamespaceEntity
{
	EntitasRedux.Tests.MyNamespaceComponent MyNamespace { get; }
	bool HasMyNamespace { get; }

	void AddMyNamespace(string newValue);
	void ReplaceMyNamespace(string newValue);
	void RemoveMyNamespace();
}
