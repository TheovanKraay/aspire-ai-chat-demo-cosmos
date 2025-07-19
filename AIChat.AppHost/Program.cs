using AIChat.AppHost;

var builder = DistributedApplication.CreateBuilder(args);


// Publish this as a Docker Compose application
builder.AddDockerComposeEnvironment("env")
       .ConfigureComposeFile(file =>
       {
           file.Name = "aspire-ai-chat";
       });

builder.AddDashboard();

// This is the AI model our application will use
var model = builder.AddAIModel("llm")
                   .RunAsOllama("tinyllama:chat", c =>
                   {
                       c.WithGPUIfSupported();
                       c.WithLifetime(ContainerLifetime.Persistent);
                   })
                   .PublishAsOpenAI("gpt-4o", b => b.AddParameter("openaikey", secret: true));


var db = builder.AddAzureCosmosDB("cosmos")
                           .RunAsPreviewEmulator(e => e.WithDataExplorer().WithDataVolume())
                           .AddCosmosDatabase("db")
                           .AddContainer("conversations", "/id");

// Redis is used to store and broadcast the live message stream
// so that multiple clients can connect to the same conversation.
var cache = builder.AddRedis("cache")
                   .WithRedisInsight();

var chatapi = builder.AddProject<Projects.ChatApi>("chatapi")
                     .WithReference(model)
                     .WaitFor(model)
                     .WithReference(db)
                     .WaitFor(db)
                     .WithReference(cache)
                     .WaitFor(cache);

builder.AddNpmApp("chatui", "../chatui")
       .WithNpmPackageInstallation()
       .WithHttpEndpoint(env: "PORT")
       .WithReverseProxy(chatapi.GetEndpoint("http"))
       .WithExternalHttpEndpoints()
       .WithOtlpExporter()
       .WithEnvironment("BROWSER", "none");

builder.Build().Run();

