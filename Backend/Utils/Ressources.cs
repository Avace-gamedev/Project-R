namespace Avace.Backend.Utils;

public static class Ressources
{
    public const string Path = "../Ressources";

    public static string MakePath(string path)
    {
        return System.IO.Path.GetFullPath(System.IO.Path.Combine(Path, path));
    }
}