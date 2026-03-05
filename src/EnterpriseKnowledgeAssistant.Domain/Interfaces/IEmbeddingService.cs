using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseKnowledgeAssistant.Domain.Interfaces
{
    public interface IEmbeddingService
    {
        Task<float[]> UploadEmbeddingAsync(string text);
        Task<float[]> QueryEmbeddingAsync(string text);
    }
}
