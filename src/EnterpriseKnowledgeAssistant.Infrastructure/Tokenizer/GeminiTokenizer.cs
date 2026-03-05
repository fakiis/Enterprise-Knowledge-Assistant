using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GenerativeAI;
using GenerativeAI.Types;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Tokenizer
{
    public class GeminiTokenizer : ITokenizer
    {
        private readonly GenerativeModel _model;

        public GeminiTokenizer(HttpClient http, string apiKey, string model = "models/gemini-1.5-flash")
        {
            var genAi = new GoogleAi(apiKey);
            _model = genAi.CreateGenerativeModel("models/gemini-2.5-flash");
        }

        public async Task<int> CountTokensAsync(string text, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var request = new CountTokensRequest
            {
                Contents = new List<Content>
                {
                    new() {
                        Role = "user",
                        Parts =
                        {
                            // 純文字直接賦值給 Text 屬性即可，不需轉換為 Blob
                            new Part { Text = text }
                        }
                    }
                }
            };
            var resp = await _model.CountTokensAsync(request, ct);

            var Tokens = resp.TotalTokens;

            return Tokens;
        }
    }
}
