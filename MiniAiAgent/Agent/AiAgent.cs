using MiniAiAgent.Models;
using MiniAiAgent.Tools;

namespace MiniAiAgent.Agent;

public class AiAgent
{
    private readonly FileTool _fileTool;
    private readonly string _projectDirectory;
    private readonly OllamaBrain _brain;
    private readonly List<ChatMessage> _conversationHistory = new();

    public AiAgent(string projectDirectory)
    {
        _fileTool = new FileTool();
        _projectDirectory = projectDirectory;
        _brain = new OllamaBrain();

        _conversationHistory.Add(new ChatMessage("system", OllamaBrain.SystemPrompt));
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Mini AI Agent is running with Memory. Type 'exit' to quit.");

        while (true)
        {
            Console.Write("\nwhat do you want to do?  --> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting AI Agent...");
                break;
            }

            await HandleRequestAsync(input);
        }
    }

    private async Task HandleRequestAsync(string input)
    {
        _conversationHistory.Add(new ChatMessage("user", input));
        Console.WriteLine("\n[Qwen3 Thinking... (Memory active)]");

        var (toolCall, responseText) = await _brain.DecideAsync(_conversationHistory);

        if (toolCall == null)
        {
            
            _conversationHistory.Add(new ChatMessage("assistant", responseText));
            return;
        }

        Console.WriteLine($"\n[Agent Decision]: The model selected the tool -> {toolCall.ToolName}");
        string toolResultData = string.Empty;

        if (toolCall.ToolName == "ListFiles")
        {
            Console.WriteLine("Files are being scanned...\n");
            var files = _fileTool.ListFiles(_projectDirectory);
            toolResultData = string.Join("\n", files);
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

            Console.WriteLine($"The file has been read: {fileName}");
            toolResultData = _fileTool.ReadFile(filePath);
        }

        var observationMessage = $"Tool '{toolCall.ToolName}' returned:\n{toolResultData}\nNow answer the user's question based on this.";
        _conversationHistory.Add(new ChatMessage("user", observationMessage));

        // --- HERE IS THE AGENT LOOP (REACT) STEP ---
        // We are sending the obtained data back to Qwen3 to get the final explanation!

        Console.WriteLine("\n[Qwen3 Analysing the data and preparing a response...]");
        var finalAnswer = await _brain.ThinkWithHistoryAsync(_conversationHistory);

        Console.WriteLine("\n[Qwen3: The Definitive Answer]:");
        Console.WriteLine(finalAnswer);

        _conversationHistory.Add(new ChatMessage("assistant", finalAnswer));
    }
}