using OllamaSharp;
using MiniAiAgent.Models;
using System.Text;

namespace MiniAiAgent.Agent;

public class OllamaBrain
{
    private readonly OllamaApiClient _ollama;

    public const string SystemPrompt =
        "You are an AI assistant in a C# project.\n" +
        "You have access to two tools:\n" +
        "1. 'ListFiles': To list all files in the project.\n" +
        "2. 'ReadFile': To read the content of a specific file.\n\n" +
        "Rules for tool calling:\n" +
        "- If the user wants to list or see project files, respond ONLY with:\n" +
        "TOOL: ListFiles\n" +
        "- If the user wants to read or see the content of a file, respond ONLY with:\n" +
        "TOOL: ReadFile | fileName: <filename>\n" +
        "Example: TOOL: ReadFile | fileName: Program.cs\n" +
        "- Otherwise, answer normally in the user's language.";

    public OllamaBrain()
    {
        var uri = new Uri("http://localhost:11434");
        _ollama = new OllamaApiClient(uri, "qwen3:4b");
    }

    public async Task<string> ThinkWithHistoryAsync(List<ChatMessage> history)
    {
        var stringBuilder = new StringBuilder();
        var promptBuilder = new StringBuilder();

        // 1. Prevent C# from automatically adding \r\n and use plain \n (Unix style) instead.
        foreach (var msg in history)
        {
            if (msg.Role == "system")
                promptBuilder.Append($"{msg.Content}\n\n");
            else if (msg.Role == "user")
                promptBuilder.Append($"User: {msg.Content}\n");
            else if (msg.Role == "assistant")
                promptBuilder.Append($"Assistant: {msg.Content}\n");
        }
        promptBuilder.Append("Assistant: "); // Tell the model "Start writing from here"

        // 2. DEFENSIVE PROGRAMMING: Infinite whitespace (Hallucination) breaker
        int emptyTokenCount = 0;

        await foreach (var response in _ollama.GenerateAsync(promptBuilder.ToString()))
        {
            if (response != null && !string.IsNullOrEmpty(response.Response))
            {
                // If the model produces only invisible characters (whitespace/newline) consecutively, increment the counter
                if (string.IsNullOrWhiteSpace(response.Response))
                {
                    emptyTokenCount++;
                    // If the model produces only whitespace 10 times in a row, break the loop to prevent locking
                    if (emptyTokenCount > 10) break;
                }
                else
                {
                    emptyTokenCount = 0; // Reset the counter when a meaningful word is produced
                }

                Console.Write(response.Response); // Display the word on the screen instantly
                stringBuilder.Append(response.Response);
            }
        }
        Console.WriteLine();
        return stringBuilder.ToString().Trim();
    }
    public async Task<(ToolCall? toolCall, string responseText)> DecideAsync(List<ChatMessage> history)
    {
        var rawResponse = await ThinkWithHistoryAsync(history);

        if (rawResponse.StartsWith("TOOL: ListFiles", StringComparison.OrdinalIgnoreCase))
        {
            return (new ToolCall { ToolName = "ListFiles" }, rawResponse);
        }

        if (rawResponse.StartsWith("TOOL: ReadFile", StringComparison.OrdinalIgnoreCase))
        {
            var call = new ToolCall { ToolName = "ReadFile" };
            if (rawResponse.Contains("fileName:"))
            {
                var parts = rawResponse.Split("fileName:", StringSplitOptions.TrimEntries);
                if (parts.Length > 1)
                {
                    call.Arguments["fileName"] = parts[1].Trim();
                }
            }
            return (call, rawResponse);
        }

        return (null, rawResponse);
    }
}