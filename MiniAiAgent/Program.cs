using MiniAiAgent.Agent;
using MiniAiAgent.Tools;

namespace MiniAiAgent
{
    internal class Program
    {
        static void Main(string[] args)
        {


            // ai agent run 

            var projectDirectory = Directory.GetParent(
                Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.Parent!.FullName;

            var agent = new AiAgent(projectDirectory);

            agent.Run();
        }
    }
}
