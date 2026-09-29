using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniAiAgent.Models;

namespace MiniAiAgent.Agent
{
    public class FakeAi
    {
        public ToolCall? Decide(string userInput)
        {
            if (userInput.Contains(
                "list files",
                StringComparison.OrdinalIgnoreCase))
            {
                return new ToolCall
                {
                    ToolName = "ListFiles"
                };
            }

            if (userInput.StartsWith(
                "read ",
                StringComparison.OrdinalIgnoreCase))
            {
                var fileName = userInput["read ".Length..].Trim();

                return new ToolCall
                {
                    ToolName = "ReadFile",
                    Arguments = new Dictionary<string, string>
                    {
                        ["fileName"] = fileName
                    }
                };
            }

            return null;
        }
    }
}
