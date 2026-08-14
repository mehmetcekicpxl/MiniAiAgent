using MiniAiAgent.Agent;
using MiniAiAgent.Tools;

namespace MiniAiAgent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // List files in the current directory
            var fileTool = new FileTool();
            var currentDirectory = Directory.GetCurrentDirectory();
            var files = fileTool.ListFiles(currentDirectory);
            foreach (var file in files)
            {
                Console.WriteLine($"File: {file}");


            }

            // ai agent run 

            var agent = new AiAgent();

            agent.Run();
        }
    }
}
