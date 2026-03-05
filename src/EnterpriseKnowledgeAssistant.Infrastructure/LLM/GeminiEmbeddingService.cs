using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using GenerativeAI;
using GenerativeAI.Types;


namespace EnterpriseKnowledgeAssistant.Infrastructure.LLM
{
    public class GeminiEmbeddingService : IEmbeddingService
    {
        private readonly EmbeddingModel _model;

        public GeminiEmbeddingService(string apiKey)
        {
            var genAi = new GoogleAi(apiKey);
            _model = genAi.CreateEmbeddingModel("models/gemini-embedding-001");
        }

        public async Task<float[]> UploadEmbeddingAsync(string text)
        {
            
            if (string.IsNullOrWhiteSpace(text)) return Array.Empty<float>();
            var result = await _model.EmbedContentAsync(new EmbedContentRequest
            {
                TaskType = TaskType.RETRIEVAL_DOCUMENT,
                Content = new Content { Parts = [new() { Text = text }] },
                OutputDimensionality = 1536
            });
            return result.Embedding.Values.ToArray();
        }

        public async Task<float[]> QueryEmbeddingAsync(string text)
        {

            if (string.IsNullOrWhiteSpace(text)) return Array.Empty<float>();
            var result = await _model.EmbedContentAsync(new EmbedContentRequest
            {
                TaskType = TaskType.RETRIEVAL_QUERY,
                Content = new Content { Parts = [new() { Text = text }] },
                OutputDimensionality = 1536
            });
            return result.Embedding.Values.ToArray();
        }
    }
}
