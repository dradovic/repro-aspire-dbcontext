var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireDbContext>("aspiredbcontext");

builder.Build().Run();
