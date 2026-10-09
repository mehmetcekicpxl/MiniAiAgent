using OllamaSharp;
using System.Text;
using MiniAiAgent.Models;

namespace MiniAiAgent.Agent;

public class OllamaBrain
{
    private readonly OllamaApiClient _ollama;

    // System instruction telling the model who it is and what tools it has at its disposal
    private const string SystemPrompt =
        "You are an AI assistant in a C# project.\n" +
        "You have access to a tool named 'ListFiles'.\n" +
        "If the user wants to see, list, or check project files, respond ONLY with:\n" +
        "TOOL: ListFiles\n" +
        "Otherwise, answer normally in the user's language.";
    public OllamaBrain()
    {
        var uri = new Uri("http://localhost:11434");
        _ollama = new OllamaApiClient(uri, "qwen3:4b");
    }

    //The place where we ask the LLM a raw question and receive a text-based answer
    public async Task<string> ThinkAsync(string userInput)
    {
        var stringBuilder = new StringBuilder();
        // We are providing both the system rule and the user's sentence to the model
        var fullPrompt = $"{SystemPrompt}\n\nUser: {userInput}\nAssistant:";

        // We are sending the user’s message to the Qwen3 model
        await foreach (var response in _ollama.GenerateAsync(fullPrompt))
        {
            if (response != null && !string.IsNullOrEmpty(response.Response))
            {
                stringBuilder.Append(response.Response);
            }
        }

        return stringBuilder.ToString();
    }
    public async Task<(ToolCall? toolCall, string responseText)> DecideAsync(string userInput)
    {
        // Ask the model to think about the user's input and return a raw response
        var rawResponse = await ThinkAsync(userInput);

        // Check if the raw response indicates a tool call
        if (rawResponse.StartsWith("TOOL: ListFiles", StringComparison.OrdinalIgnoreCase))
        {
            var call = new ToolCall
            {
                ToolName = "ListFiles"
            };
            return (call, rawResponse);
        }

        // If no tool was requested, ToolCall is empty (null); there is only chat text
        return (null, rawResponse);
    }
}