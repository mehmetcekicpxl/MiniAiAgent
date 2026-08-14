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
            return Directory.GetFiles(
                directory,"*",SearchOption.AllDirectories);
        }

        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
