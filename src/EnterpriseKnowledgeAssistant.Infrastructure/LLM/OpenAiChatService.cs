using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using OpenAI;
using OpenAI.Chat;

namespace EnterpriseKnowledgeAssistant.Infrastructure.LLM
{
    public class OpenAiChatService : ILLMService
    {
        private readonly ChatClient _client;
        private const string Model = "gpt-4o-mini";

        public OpenAiChatService(string apiKey)
        {
            _client = new ChatClient(
                model: Model,  
                apiKey: apiKey
            );
        }

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            var messages = new List<ChatMessage>()
            {
                new SystemChatMessage("You are a helpful enterprise assistant."),
                new UserChatMessage(prompt)
            };
            // 呼叫 CompleteChatAsync
            ChatCompletion result = await _client.CompleteChatAsync(messages, /* options */ null);

            return result.Content[0].Text;
        }
    }
}
