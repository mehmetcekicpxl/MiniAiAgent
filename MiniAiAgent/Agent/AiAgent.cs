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
        private readonly OllamaBrain _brain;// Add this line to declare the OllamaBrain instance

        public AiAgent(string projectDirectory)
        {
            _fakeAi = new FakeAi();
            _fileTool = new FileTool();
            _projectDirectory = projectDirectory;
            _brain = new OllamaBrain(); // Initialize the OllamaBrain instance
        }
        public async Task RunAsync()
        {
           Console.WriteLine("Mini AI Agent is running. Type 'exit' to quit.");
           
           while (true)
            {
                Console.Write("\nwat will je doen?  -->");
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
                await HandleRequestAsync(input);
            }

        }
        private async Task HandleRequestAsync(string input)
        {
            var toolCall = _fakeAi.Decide(input);

            // If FakeAi cannot generate a Tool prompt, we ask Ollama directly
            if (toolCall == null)
            {
                Console.WriteLine("\n[Fake AI did not recognise this request -> Qwen3 is thinking...]");
                var aiResponse = await _brain.ThinkAsync(input);
                Console.WriteLine("\n[Qwen3's answer]:");
                Console.WriteLine(aiResponse);
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

            if (toolCall.ToolName == "ReadFile")
            {
                if (!toolCall.Arguments.TryGetValue("fileName", out var fileName))
                {
                    Console.WriteLine("File name is missing.");
                    return;
                }

                var filePath = Path.Combine(_projectDirectory, fileName);
                var content = _fileTool.ReadFile(filePath);

                Console.WriteLine("File content:");
                Console.WriteLine(content);
            }
        }
    }
}
