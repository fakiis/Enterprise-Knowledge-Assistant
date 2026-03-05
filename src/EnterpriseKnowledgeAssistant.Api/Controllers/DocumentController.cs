using Microsoft.AspNetCore.Mvc;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Application.DTOs;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using Pgvector;

namespace EnterpriseKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IEmbeddingService _embeddingService;

        public DocumentController(AppDbContext db, IEmbeddingService embeddingService)
        {
            _db = db;
            _embeddingService = embeddingService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(DocumentUploadDto dto)
        {
            var doc = new Document(dto.Title, dto.Content);

            var embedding = await _embeddingService.UploadEmbeddingAsync(dto.Content);
            var _vector = new Vector(embedding);
            doc.UpdateEmbedding(_vector);

            _db.Documents.Add(doc);
            await _db.SaveChangesAsync();

            return Ok(new { doc.Id });
        }

        [HttpGet]
        public IActionResult List()
        {
            var docs = _db.Documents
                          .Select(d => new { d.Id, d.Title, d.CreatedAt });

            return Ok(docs);
        }
    }
}
