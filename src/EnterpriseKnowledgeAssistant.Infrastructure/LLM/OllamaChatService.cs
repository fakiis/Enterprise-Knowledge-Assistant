using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EnterpriseKnowledgeAssistant.Application.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.LLM
{
    public class OllamaChatService : ILLMService
    {
        private readonly HttpClient _http;

        public OllamaChatService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            var response = await _http.PostAsJsonAsync(
                "/api/chat",
                new
                {
                    model = "llama3",
                    messages = new[]
                    {
                    new { role = "user", content = prompt }
                    },
                    stream = false
                });

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            return json
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
