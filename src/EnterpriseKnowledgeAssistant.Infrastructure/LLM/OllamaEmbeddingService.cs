using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Application.DTOs;

namespace EnterpriseKnowledgeAssistant.Infrastructure.LLM
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _http;

        public OllamaEmbeddingService(HttpClient http)
        {
            _http = http;
        }

        public async Task<float[]> UploadEmbeddingAsync(string text)
        {
            var response = await _http.PostAsJsonAsync(
                "/api/embeddings",
                new
                {
                    model = "bge-m3",
                    prompt = text
                });

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<OllamaEmbeddingResponse>();

            return result.Embeddings;
        }

        public async Task<float[]> QueryEmbeddingAsync(string text)
        {
            var response = await _http.PostAsJsonAsync(
                "/api/embeddings",
                new
                {
                    model = "bge-m3",
                    prompt = text
                });

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<OllamaEmbeddingResponse>();

            return result.Embeddings;
        }
    }
}
