using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseKnowledgeAssistant.Domain.Interfaces
{
    public interface ITokenBasedChunker
    {
        Task<List<string>> GetChunksAsync(string text, CancellationToken ct = default);
    }
}
