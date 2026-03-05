# Enterprise Knowledge Assistant Platform
Current Progress.

## Overview

An enterprise-ready knowledge assistant platform based on ASP.NET Core 8 and LLM technologies, PostgreSQL.

## Tech Stack

- ASP.NET Core 8
- PostgreSQL
- pgvector
- Docker
- Google Gemini API
- OpenAI API / Local LLM (planned)

## Architecture

Clean Architecture:

API
Application
Domain
Infrastructure

## Features

- Document upload
- Embedding generation
- Vector search
- LLM-based Q&A

## Planning Features

- Document ingestion
- Chunk-based indexing

## Running with Docker

Fill in your API key and Database setting to appsettings.json

```bash
docker compose up --build
```