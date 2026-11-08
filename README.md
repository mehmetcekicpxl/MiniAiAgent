# MiniAiAgent 🤖 | Autonomous Local AI Agent Engine in .NET 8

MiniAiAgent is an autonomous, privacy-first AI Agent framework engineered from scratch in **C#** and **.NET 8**, powered by local LLM execution via **Ollama (Qwen3:4b)** and **OllamaSharp**.

Unlike standard chatbot wrappers that simply echo prompt completions, MiniAiAgent implements a genuine **ReAct (Reason + Act)** agent loop. The system inspects natural language input, makes deterministic tool-calling decisions, extracts structured arguments, executes C# backend services against local resources, and feeds execution results back into the model for final contextual synthesis.

---

## 🌟 Architectural Highlights

- **100% Local & Privacy-Preserving:** Powered by local Ollama (`qwen3:4b`) inference—no external API keys, zero cloud latency/costs, and guaranteed data privacy.
- **Deterministic Tool Calling:** Leverages system prompt engineering to guide the LLM into producing structured command contracts (`TOOL: <Name> | <Params>`).
- **Dynamic Argument Parsing:** Extracts runtime arguments (such as target file paths) and maps them into strongly-typed `ToolCall` DTO models.
- **Full ReAct Agent Loop:** Completes the *Reasoning ➔ Acting ➔ Observation ➔ Synthesis* loop by feeding tool execution outputs back into the model context before answering.
- **Clean Architecture & Separation of Concerns:**
  - **`AiAgent` (Orchestrator):** Manages user session, loop transitions, and tool dispatching.
  - **`OllamaBrain` (Cognitive Layer):** Interfaces with Ollama, triggers inference, parses structured decisions, and synthesizes tool observations.
  - **`FileTool` (Execution Engine):** Implements localized system operations while filtering build noise (`bin`, `obj`, `.git`, `.vs`).
  - **`ToolCall` (Domain Model):** Decoupled DTO representing tool intent and argument dictionaries.

---

## 🏛️ System Architecture & Workflow

```text
               User Prompt
                    │
                    ▼
          ┌───────────────────┐
          │      AiAgent      │  (Orchestrator)
          └─────────┬─────────┘
                    │
                    ▼
          ┌───────────────────┐
          │    OllamaBrain    │  (Qwen3:4b Reasoning)
          └─────────┬─────────┘
                    │
         ┌──────────┴──────────┐
         │                     │
   [Direct Chat]        [Tool Invocation]
         │                     │
         ▼                     ▼
   Return Text        ┌─────────────────┐
                      │    ToolCall     │  (DTO: ToolName + Arguments)
                      └────────┬────────┘
                               │
                               ▼
                      ┌─────────────────┐
                      │    FileTool     │  (C# Execution Layer)
                      └────────┬────────┘
                               │
                               ▼ [Observation: File Content / Directory Tree]
                      ┌─────────────────┐
                      │   OllamaBrain   │  (Final Context Synthesis)
                      └────────┬────────┘
                               │
                               ▼
                       Synthesized Answer
```

---

## 🛠️ Tech Stack & Prerequisites

- **Language & Runtime:** C# 12 / .NET 8.0 SDK
- **Local LLM Engine:** [Ollama](https://ollama.com/) (v0.35+)
- **Active Model:** `qwen3:4b`
- **Integration Library:** [OllamaSharp](https://github.com/awaescher/OllamaSharp)
- **IDE:** Visual Studio 2022 / VS Code / JetBrains Rider

---

## 🚀 Step-by-Step Learning Roadmap

This project is built incrementally to master end-to-end AI Agent engineering:

- [x] **Phase 1: Local Ollama Connectivity & Streaming** (`IAsyncEnumerable<T>` integration)
- [x] **Phase 2: Asynchronous Agent Pipeline** (`Task`-based orchestrator loop)
- [x] **Phase 3: Structured System Prompting** (Constrained tool-calling grammar)
- [x] **Phase 4: Parameterized Tool Calling** (Dynamic argument extraction: `fileName`)
- [x] **Phase 5: ReAct Agent Loop** (Feeding tool execution observations back to LLM)
- [ ] **Phase 6: Conversational Memory & Context Window Management** *(Current)*
- [ ] **Phase 7: Self-Correction & Robust Parsing Safeguards**
- [ ] **Phase 8: Human-in-the-Loop Security & Approval Gates (File Mutation/Delete)**
- [ ] **Phase 9: Autonomous Git Tooling (Diff, Commit, Branching)**
- [ ] **Phase 10: Local Codebase RAG via Vector Embeddings**
- [ ] **Phase 11: Model Context Protocol (MCP) Integration**
- [ ] **Phase 12: Multi-Agent Collaboration & Review Workflows**

---

### 💻 Live Console Demonstration

#### 1. Natural Language Tool Invocation & Argument Parsing (Dutch / Flemish)
```text
Mini AI Agent is running. Type 'exit' to quit.

what do you want to do?  --> lees Program.cs en leg uit wat het doet

[Qwen3 Thinking...]

[Agent Decision]: Model chose the following tool -> ReadFile
Reading file: Program.cs

[Qwen3 Inspecting Data & Generating Final Answer...]

[Qwen3 Final Response]:
Het bestand Program.cs dient als het primaire instappunt (entry point) van de applicatie:

1. **Projectdirectory bepalen**: Navigeert drie niveaus omhoog vanaf de huidige werkdirectory om de root van de solution te lokaliseren.
2. **AiAgent initialiseren**: Maakt een instantie van `AiAgent` aan met het gevonden pad als werkcontext.
3. **Asynchrone loop starten**: Roept `await agent.RunAsync()` aan om de ReAct-interactielus en console-invoer te starten.

what do you want to do?  --> how are you doing today?

[Qwen3 Thinking...]

[Qwen3 Chat]:
I am doing great, thank you! I am ready to inspect project files or answer questions about your C# codebase.
```
---


##  📂 Project Structure
```text
MiniAiAgent/
│
├── Agent/
│   ├── AiAgent.cs         # Main agent orchestrator and CLI REPL loop
│   ├── FakeAi.cs          # Mock baseline agent used for architectural validation
│   └── OllamaBrain.cs     # Reasoning engine, prompt contracts, and inference pipeline
│
├── Models/
│   └── ToolCall.cs        # Strongly-typed tool invocation model (Name + Arguments)
│
├── Tools/
│   └── FileTool.cs        # Secure file system operations with artifact exclusions
│
├── OllamaTest.cs          # Direct API connection and streaming diagnostics
└── Program.cs             # Application bootstrapper and path resolution
```

## ⚡ Getting Started Locally

### 1. Clone the Repository
```bash
git clone https://github.com/mehmetcekicpxl/AI_AGENT_VOORBEREIDEN_PROJECTEN.git
cd AI_AGENT_VOORBEREIDEN_PROJECTEN/MiniAiAgent
```

### 2. Start Ollama and Pull the Model
Ensure Ollama is running locally on port `11434`:
```bash
ollama pull qwen3:4b
```

### 3. Run the Application
```bash
dotnet build
dotnet run --project MiniAiAgent/MiniAiAgent.csproj
```

---

## 👤 Author

**Mehmet Çekiç**  
- GitHub: [@mehmetcekicpxl](https://github.com/mehmetcekicpxl)  
- Focus: Software Architecture, Clean Code (.NET 8 / Blazor), and Autonomous AI Agent Systems.
