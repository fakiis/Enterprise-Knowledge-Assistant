using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Tokenizers
{
    public class HeuristicTokenizer : ITokenizer
    {
        public Task<int> CountTokensAsync(string text, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(text))
                return Task.FromResult(0);

            // 超粗估：英文約 4 chars/token，中文約 1.5~2 chars/token
            var length = text.Length;

            int estimated = (int)(length / 3.5);
            return Task.FromResult(Math.Max(1, estimated));
        }
    }
}
