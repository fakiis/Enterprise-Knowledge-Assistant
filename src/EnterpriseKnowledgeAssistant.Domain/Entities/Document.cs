using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgvector;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }

        // 向量先用 float[] 表示，實作在 Infrastructure
        [Column(TypeName = "vector(1536)")]
        public Vector Embedding { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Document() { } // For ORM

        public Document(string title, string content)
        {
            Id = Guid.NewGuid();
            Title = title;
            Content = content;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmbedding(Vector embedding)
        {
            Embedding = embedding;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
