using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Embeddings;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;

namespace EnterpriseKnowledgeAssistant.Infrastructure.LLM
{
    public class OpenAiEmbeddingService : IEmbeddingService
    {
        private readonly EmbeddingClient _client;
        private const string Model = "text-embedding-3-small"; // 1536 dims

        public OpenAiEmbeddingService(string apiKey)
        {
            _client = new EmbeddingClient(
                model: Model,  
                apiKey: apiKey  
            );
        }

        public async Task<float[]> UploadEmbeddingAsync(string text)
        {
            var response = await _client.GenerateEmbeddingsAsync(new[] { text });
            return response.Value[0].ToFloats().ToArray();
        }

        public async Task<float[]> QueryEmbeddingAsync(string text)
        {
            var response = await _client.GenerateEmbeddingsAsync(new[] { text });
            return response.Value[0].ToFloats().ToArray();
        }
    }
}
