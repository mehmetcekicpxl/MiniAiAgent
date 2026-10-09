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
                Console.Write("\nwhat do you want to do?  -->");
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
            Console.WriteLine("\n[Qwen3 Thinking...]");
            var aiResponse = await _brain.ThinkAsync(input);
            var (toolCall, responseText) = await _brain.DecideAsync(input);

            // if its not a tool call, we just print the response
            if (toolCall == null)
            {
                Console.WriteLine("\n[Qwen3 Chat]:");
                Console.WriteLine(responseText);
                return;
            }

            if (aiResponse.StartsWith("TOOL: ListFiles", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"\n[Agent Decision]:The model selected the following tool -> {toolCall.ToolName}");
                if (toolCall.ToolName.Equals("ListFiles", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Scanning files...\n");
                    // We are running our C# tool
                    var files = _fileTool.ListFiles(_projectDirectory);

                    Console.WriteLine("Found Files:");
                    foreach (var file in files)
                    {
                        Console.WriteLine($"- {file}");
                    }
                }
                   

                
                return;
            }

            // 2. If no tool was requested, this is a normal chat response
            Console.WriteLine("\n[Qwen3 Response]:");
            Console.WriteLine(aiResponse);
        }
    }
}
