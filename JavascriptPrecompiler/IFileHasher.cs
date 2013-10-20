namespace JavascriptPrecompiler
{
	public interface IFileHasher
	{
		string GetHash(string fileContents);
	}
}