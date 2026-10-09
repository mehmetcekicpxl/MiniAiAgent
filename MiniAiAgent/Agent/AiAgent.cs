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

            // 3. Model chose a tool!
            Console.WriteLine($"\n[Agent Decision]: Model chose the following tool -> {toolCall.ToolName}");

            string toolResultData = string.Empty;

            if (toolCall.ToolName == "ListFiles")
            {
                Console.WriteLine("Scanning files...\n");
                var files = _fileTool.ListFiles(_projectDirectory);
                // We are combining the list of files into a single text document
                toolResultData = string.Join("\n", files);

                Console.WriteLine("Found Files:");
                foreach (var file in files)
                {
                    Console.WriteLine($"- {file}");
                }
            }
            else if (toolCall.ToolName == "ReadFile")
            {
                if (!toolCall.Arguments.TryGetValue("fileName", out var fileName) || string.IsNullOrWhiteSpace(fileName))
                {
                    Console.WriteLine("Error: The model file name was not specified.");
                    return;
                }

                var filePath = Path.Combine(_projectDirectory, fileName);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: The file '{fileName}' could not be found.");
                    return;
                }

                Console.WriteLine($"File read: {fileName}");
                toolResultData = _fileTool.ReadFile(filePath);
            }

            // --- HERE IS THE AGENT LOOP (REACT) STEP ---
            // We are sending the obtained data back to Qwen3 to get the final explanation!
            Console.WriteLine("\n[Qwen3 Analyzing Data and Preparing Response...]");
            var finalAnswer = await _brain.AnswerWithToolResultAsync(input, toolResultData);

            Console.WriteLine("\n[Qwen3 Final Answer]:");
            Console.WriteLine(finalAnswer);

        }
    }
}
