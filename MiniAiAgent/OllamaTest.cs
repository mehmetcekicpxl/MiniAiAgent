using OllamaSharp;
using System.Text;

namespace MiniAiAgent;

public class OllamaTest
{
    private readonly OllamaApiClient _ollama;
    public OllamaTest()
    {
        var uri = new Uri("http://localhost:11434");
        _ollama = new OllamaApiClient(uri, "qwen3:4b");
    }
    public async Task<string> AskAsync(string prompt)
    {
        var stringBuilder = new StringBuilder();

        await foreach (var response in _ollama.GenerateAsync(prompt))
        {
            if (response != null && !string.IsNullOrEmpty(response.Response))
            {
                // We add every small chunk that comes in
                stringBuilder.Append(response.Response);

                // We tap the screen to view the process and see how the response is displayed
                Console.Write(response.Response);
            }
        }

        Console.WriteLine(); // Once you’ve finished your answer, move on to the next line
        return stringBuilder.ToString();
    }
}