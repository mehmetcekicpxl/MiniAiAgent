using OllamaSharp;
using System.Text;

namespace MiniAiAgent.Agent;

public class OllamaBrain
{
    private readonly OllamaApiClient _ollama;

    public OllamaBrain()
    {
        var uri = new Uri("http://localhost:11434");
        _ollama = new OllamaApiClient(uri, "qwen3:4b");
    }

    public async Task<string> ThinkAsync(string userInput)
    {
        var stringBuilder = new StringBuilder();

        // We are sending the user’s message to the Qwen3 model
        await foreach (var response in _ollama.GenerateAsync(userInput))
        {
            if (response != null && !string.IsNullOrEmpty(response.Response))
            {
                stringBuilder.Append(response.Response);
            }
        }

        return stringBuilder.ToString();
    }
}