using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseKnowledgeAssistant.Domain.Interfaces
{
    public interface ITokenizer
    {
        Task<int> CountTokensAsync(string text, CancellationToken ct = default);
    }
}
