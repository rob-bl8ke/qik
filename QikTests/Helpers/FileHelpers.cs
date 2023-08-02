using System.IO;

namespace QikTests
{
    public class FileHelpers
    {
        public static string GetFolder() => "..\\..\\..\\..\\QikTests\\Files";
        public static string GetSubFolder(string appendedPath) => Path.Combine(GetFolder(), appendedPath);
        public static string ResolvePath(string fileName) => Path.Combine(GetFolder(), fileName);
        public static string ReadText(string fileName) => File.ReadAllText(ResolvePath(fileName));
        public static void DeleteFile(string fileName) => File.Delete(ResolvePath(fileName));
        public static void DeleteDirectory(string directoryName) => Directory.Delete(ResolvePath(directoryName), recursive: true);
    }
}
