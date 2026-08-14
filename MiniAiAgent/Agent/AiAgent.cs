using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAiAgent.Agent
{
    public class AiAgent
    {
        public void Run()
        {
           // Console.WriteLine("AI Agent is running...");
           // Add your AI agent logic here
           while(true)
            {
                Console.Write("wat will je doen?  -->   ");
                var input= Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                if(input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting AI Agent...");
                    break;
                }
                Console.WriteLine($"You entered: {input}");
            }

        }
    }
}
