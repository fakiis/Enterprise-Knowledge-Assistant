using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Document> Documents => Set<Document>();


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("documents");

                entity.HasKey(d => d.Id);

                entity.Property(d => d.Title)
                      .IsRequired();

                entity.Property(d => d.Content)
                      .IsRequired();

                entity.Property(d => d.Embedding)
                      .HasColumnType("vector(1536)");

                entity.Property(d => d.CreatedAt);
                entity.Property(d => d.UpdatedAt);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
