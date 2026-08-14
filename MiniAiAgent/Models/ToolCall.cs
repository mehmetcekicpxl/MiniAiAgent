using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAiAgent.Models
{
    public class ToolCall
    {
        public string ToolName { get; set; } = string.Empty;
        public Dictionary<string, string> Arguments { get; set; } = new();

    }
}
