var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.StudentRegistry>("studentregistry");

builder.Build().Run();
