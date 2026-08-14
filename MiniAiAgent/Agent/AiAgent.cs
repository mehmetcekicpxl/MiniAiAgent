using MiniAiAgent.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAiAgent.Agent
{
    public class AiAgent
    {
        private readonly FakeAi _fakeAi;
        private readonly FileTool _fileTool;
        private readonly string _projectDirectory;

        public AiAgent(string projectDirectory)
        {
            _fakeAi = new FakeAi();
            _fileTool = new FileTool();
            _projectDirectory = projectDirectory;
        }
        public void Run()
        {
           Console.WriteLine("Mini AI Agent is running. Type 'exit' to quit.");
           
           while (true)
            {
                Console.Write("wat will je doen?  -->");
                var input= Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                if(input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting AI Agent...");
                    break;
                }
                HandleRequest(input);
            }

        }
        private void HandleRequest(string input)
        {
            var toolCall =_fakeAi.Decide(input);

            if(toolCall == null)
            {
                Console.WriteLine("Fake AI does not know what to do.");
                return;
            }
            Console.WriteLine($"Fake AI wants to use: {toolCall.ToolName}");

            if (toolCall.ToolName == "ListFiles")
            {
                

                var files = _fileTool.ListFiles(_projectDirectory);

                Console.WriteLine("Files found:");

                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }
        }
    }
}
