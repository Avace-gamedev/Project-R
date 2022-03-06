using System.IO;
using System.Reflection;

namespace Tests.Utils
{
    public static class TestUtils
    {
        public static string GetTestFile(string testFilePath)
        {
            Assembly executingAssembly = Assembly.GetCallingAssembly();
            string location = Path.GetDirectoryName(executingAssembly.Location)!;
            string assemblyName = Path.GetFileNameWithoutExtension(executingAssembly.Location);
            return Path.Combine(location, $"../../../../{assemblyName}/TestFiles", testFilePath);
        }
    }
}