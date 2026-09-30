using OllamaSharp;

namespace MiniAiAgent;

public class OllamaTest
{
    public async Task RunAsync()
    {
        var uri = new Uri("http://localhost:11434");

        var ollama = new OllamaApiClient(uri, "qwen3:4b");

        Console.WriteLine("Sending message to Ollama...");

        Console.WriteLine();
        Console.WriteLine("Ollama response:");

        await foreach (var response in ollama.GenerateAsync(
            "Hello! Introduce yourself in one short sentence."))
        {
            Console.Write(response?.Response);
        }
    }
}