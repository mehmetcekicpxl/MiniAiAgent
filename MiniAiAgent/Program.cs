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

            await ollamaTest.RunAsync();

            var projectDirectory = Directory.GetParent(
                Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

            var agent = new AiAgent(projectDirectory);

            agent.Run();
        }
    }
}
