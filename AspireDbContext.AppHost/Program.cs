var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");
var sqldb = sql.AddDatabase("sqldb");

builder.AddProject<Projects.AspireDbContext>("aspiredbcontext")
    .WithReference(sqldb);

builder.Build().Run();
