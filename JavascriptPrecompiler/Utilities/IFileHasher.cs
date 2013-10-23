namespace JavascriptPrecompiler.Utilities
{
	public interface IFileHasher
	{
		string GetHash(string fileContents);
	}
}