using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EnterpriseKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/query")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;
        private readonly AppDbContext _db;
        private readonly IEmbeddingService _embeddingService;
        private readonly ILLMService _llmService;

        public QueryController(ILogger<QueryController> logger, AppDbContext db, IEmbeddingService embeddingService, ILLMService llmService)
        {
            _db = db;
            _logger = logger;
            _embeddingService = embeddingService;
            _llmService = llmService;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] string question)
        {
            _logger.LogInformation("User question: {Question}", question);
            var queryEmbeddingArray = await _embeddingService.QueryEmbeddingAsync(question);
            _logger.LogInformation("Query embedding length: {Len}", queryEmbeddingArray.Length);

            // 將 float[] 轉為 pgvector Vector
            Vector queryVector = new(queryEmbeddingArray);

            var docs = await _db.Documents
                .OrderBy(d => d.Embedding.CosineDistance(queryVector))
                .Take(5)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} documents", docs.Count);

            foreach (var d in docs)
            {
                _logger.LogInformation("DocId={Id}, Content={Content}", d.Id, d.Content);
            }

            var context = string.Join("\n---\n", docs.Select(d => d.Content));

            _logger.LogInformation("Context sent to LLM:\n{Context}", context);

            var prompt = $"""
                        You are a retrieval-based assistant.

                        Rules:
                        - You MUST answer using ONLY the provided context.

                        Use the following context to answer the question.

                        Context:
                        {context}

                        Question:
                        {question}
                        """;

            var answer = await _llmService.GenerateAnswerAsync(prompt);

            _logger.LogInformation("LLM Answer: {Answer}", answer);

            return Ok(new { answer });
        }
    }

}
