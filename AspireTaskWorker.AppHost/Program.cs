var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireTaskWorker_ApiService>("apiservice")
    .WithReplicas(1);

builder.AddProject<Projects.AspireTaskWorker_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WithReplicas(1);

builder.Build().Run();
