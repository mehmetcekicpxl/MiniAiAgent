using MiniAiAgent.Tools;

namespace MiniAiAgent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileTool = new FileTool();
            var currentDirectory = Directory.GetCurrentDirectory();
            var files = fileTool.ListFiles(currentDirectory);
            foreach (var file in files)
            {
                Console.WriteLine($"File: {file}");


            }
        }
    }
}
