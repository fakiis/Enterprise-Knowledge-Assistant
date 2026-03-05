using System;
using Microsoft.EntityFrameworkCore;
using EnterpriseKnowledgeAssistant.Infrastructure.Persistence;
using EnterpriseKnowledgeAssistant.Infrastructure.LLM;
using EnterpriseKnowledgeAssistant.Application.Interfaces;
using EnterpriseKnowledgeAssistant.Domain.Interfaces;
using EnterpriseKnowledgeAssistant.Api.Options;
using Pgvector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Configuration
    .AddEnvironmentVariables();
builder.Services.AddSwaggerGen();

var GEMINI_API_KEY = builder.Configuration["Gemini:ApiKey"] ?? builder.Configuration["GEMINI_API_KEY"];
var OPENAI_API_KEY = builder.Configuration["OpenAI:ApiKey"] ?? builder.Configuration["OPENAI_API_KEY"];

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector()
    )
    .UseSnakeCaseNamingConvention()
);

var aiOptions = builder.Configuration
    .GetSection("AI")
    .Get<AIOptions>()!;

if (aiOptions.EmbeddingProvider == "Gemini")
{
    builder.Services.AddScoped<IEmbeddingService>(_ =>
        new GeminiEmbeddingService(
            GEMINI_API_KEY!));
}
else
{
    builder.Services.AddScoped<IEmbeddingService>(_ =>
        new OpenAiEmbeddingService(
            OPENAI_API_KEY!));
}

// Chat ¥i¿W¥ß¤Á
if (aiOptions.ChatProvider == "Gemini")
{
    builder.Services.AddScoped<ILLMService>(_ =>
        new GeminiChatService(
            GEMINI_API_KEY!));
}
else
{
    builder.Services.AddScoped<ILLMService>(_ =>
        new OpenAiChatService(
            OPENAI_API_KEY!));
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
