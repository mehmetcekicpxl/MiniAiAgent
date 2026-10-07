using MiniAiAgent.Agent;
using MiniAiAgent.Tools;

namespace MiniAiAgent
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


            // ai agent run 
            var ollamaTest = new OllamaTest();

            var answer = await ollamaTest.AskAsync("What is 2 + 2?");

            var projectDirectory = Directory.GetParent(
                Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

            var agent = new AiAgent(projectDirectory);

            agent.Run();
        }
    }
}
