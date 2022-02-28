using System.IO;
using FluentAssertions;
using Newtonsoft.Json;

namespace Tests.Utils
{
    public static class DumpComparer
    {
        public static void ShouldBeEquivalentToDumpFile(this object obj, string path)
        {
#if DEBUG
            if (!File.Exists(path))
            {
                File.WriteAllText(path, Serialize(obj));
                throw new FileNotFoundException($"Could not find {path}");
            }
#else
        path.Should().Match(s => File.Exists(s));
#endif

            string content = File.ReadAllText(path);
#if DEBUG
            File.WriteAllText(path, Serialize(obj));
#endif
            obj.ShouldBeEquivalentToDump(content);
        }

        public static void ShouldBeEquivalentToDump(this object obj, string dump)
        {
            Serialize(obj).Should().Be(dump);
        }

        private static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
