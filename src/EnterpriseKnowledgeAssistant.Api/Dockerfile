FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/EnterpriseKnowledgeAssistant.Api/EnterpriseKnowledgeAssistant.Api.csproj", "src/EnterpriseKnowledgeAssistant.Api/"]

RUN dotnet restore "src/EnterpriseKnowledgeAssistant.Api/EnterpriseKnowledgeAssistant.Api.csproj"

COPY . .

WORKDIR "/src/src/EnterpriseKnowledgeAssistant.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnterpriseKnowledgeAssistant.Api.dll"]