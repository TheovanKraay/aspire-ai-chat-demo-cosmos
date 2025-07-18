# Aspire AI Chat

Aspire AI Chat is a full-stack chat sample that combines modern technologies to deliver a ChatGPT-like experience.

# What is Aspire?

[Aspire](https://learn.microsoft.com/dotnet/aspire/) is a new cloud-native stack that streamlines the development, orchestration, and observability of distributed applications. With Aspire, you can simply start debugging, and it will dynamically spin up containers, configure services, and launch a unified dashboard - complete with full observability and integrated debugging support. In this sample, Aspire runs the entire AI chat system locally (including the backend API, Azure Cosmos DB emulator, Ollama, and React frontend) exactly as it would run in production. No other framework delivers Aspire’s seamless "F5 = full cloud-native stack + observability + debug" workflow. It enables a tightly integrated, cloud-native development model that makes working with microservices feel as straightforward as building a monolith.

## High-Level Overview

- **Backend API:**  
  The backend is built with **ASP.NET Core** and interacts with an LLM using **Microsoft.Extensions.AI**. It leverages `IChatClient` to abstract the interaction between the API and the model. Chat responses are streamed back to the client using stream JSON array responses.

- **Data & Persistence:**  
  Uses **Entity Framework Core** with **Azure Cosmos DB** for flexible, cloud-based NoSQL storage. This project utilizes the [**new preview CosmosDB emulator**](https://learn.microsoft.com/azure/cosmos-db/emulator-linux) for efficient local development.

- **AI & Chat Capabilities:**  
  - Uses **Ollama** (via OllamaSharp) for local inference, enabling context-aware responses.  
  - In production, the application switches to [**OpenAI**](https://openai.com/) for LLM capabilities.

- **Frontend UI:**  
  Built with **React**, the user interface offers a modern and interactive chat experience. The React application is built and hosted using [**Caddy**](https://caddyserver.com/).

## Getting Started

### Prerequisites

- [.NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started) or [Podman](https://podman-desktop.io/)
- [Node.js](https://nodejs.org/) (LTS version recommended)

### Running the Application

Run the [AIChat.AppHost](AIChat.AppHost) project. This project uses  
[.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)  
to run the application in a container.

### Configuration

- By default, the application uses **Ollama** for local inference.  
- To use **OpenAI**, set the appropriate configuration values (e.g., API keys, endpoints).  
- The Azure Cosmos DB database will be automatically created and migrated when running with Aspire.

