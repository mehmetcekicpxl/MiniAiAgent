using MiniAiAgent.Agent;
using MiniAiAgent.Tools;

namespace MiniAiAgent
{
    internal class Program
    {
        static async Task Main(string[] args)
        {



            var projectDirectory = Directory.GetParent(
                Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

            var agent = new AiAgent(projectDirectory);

            await agent.RunAsync();
        }
    }
}
