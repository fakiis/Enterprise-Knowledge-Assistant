using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerativeAI;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using EnterpriseKnowledgeAssistant.Application.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.LLM
{
    public class GeminiChatService : ILLMService
    {
        private readonly GenerativeModel _model;

        public GeminiChatService(string apiKey)
        {
            var genAi = new GoogleAi(apiKey);
            _model = genAi.CreateGenerativeModel("models/gemini-2.5-flash");
        }

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            var response = await _model.GenerateContentAsync(prompt);

            // 取第一個候選回覆的文字
            var text = response.Candidates
                               .FirstOrDefault()?
                               .Content?
                               .Parts?
                               .FirstOrDefault()?
                               .Text;

            return text ?? string.Empty;
        }
    }
}
