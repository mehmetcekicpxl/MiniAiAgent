using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAiAgent.Tools
{
    public class FileTool
    {
        public string[] ListFiles(string directory)
        {
            var excludedDirectories = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase)
    {
        ".git",
        ".gitattributes",
        ".gitignore",
        "README.md",
        ".vs",
        "bin",
        "obj"
    };

            return Directory
                .EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                .Where(file =>
                {
                    var relativePath = Path.GetRelativePath(directory, file);

                    var parts = relativePath.Split(
                        Path.DirectorySeparatorChar,
                        Path.AltDirectorySeparatorChar);

                    return !parts.Any(part => excludedDirectories.Contains(part));
                })
                .ToArray();
        }

        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
