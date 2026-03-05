using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using GenerativeAI;
using GenerativeAI.Types;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Chunker
{
    public class GeminiChunker : ITokenBasedChunker
    {
        private readonly ITokenizer _tokenizer;
        private readonly int _maxTokens;

        public GeminiChunker(ITokenizer tokenizer, int maxTokens = 800)
        {
            _tokenizer = tokenizer;
            _maxTokens = maxTokens;
        }

        public async Task<List<string>> GetChunksAsync(string text, CancellationToken ct = default)
        {
            var parts = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var chunks = new List<string>();
            var sb = new StringBuilder();
            var currentTokens = 0;

            foreach (var part in parts)
            {
                var tokens = await _tokenizer.CountTokensAsync(part, ct);

                if (currentTokens + tokens > _maxTokens && sb.Length > 0)
                {
                    chunks.Add(sb.ToString());
                    sb.Clear();
                    currentTokens = 0;
                }

                sb.AppendLine(part);
                currentTokens += tokens;
            }

            if (sb.Length > 0)
                chunks.Add(sb.ToString());

            return chunks;
        }
    }
}
