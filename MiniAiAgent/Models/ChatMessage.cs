namespace MiniAiAgent.Models;

public class ChatMessage
{
    public string Role { get; set; } = string.Empty; // "system", "user" or "assistant"
    public string Content { get; set; } = string.Empty;

    public ChatMessage(string role, string content)
    {
        Role = role;
        Content = content;
    }
}