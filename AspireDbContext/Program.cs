// See https://aka.ms/new-console-template for more information
using AspireDbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.AddSqlServerDbContext<BloggingContext>("sqldb");

using IHost host = builder.Build();
host.Start();

ExecuteDb(db => db.Database.EnsureCreated());

// Create
Console.WriteLine("Inserting a new blog");
ExecuteDb(db =>
{
    db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
    db.SaveChanges();
});

// Read
ListBlogs();

// Update
Console.WriteLine("Updating the blog and adding a post");
ExecuteDb(db =>
{
    var blog = db.Blogs
        .OrderBy(b => b.BlogId)
        .First();
    blog.Url = "https://devblogs.microsoft.com/dotnet";
    blog.Posts.Add(new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
    db.SaveChanges();
});

ListBlogs();


// Delete
Console.WriteLine("Delete the blog");
ExecuteDb(db =>
{
    var blog = db.Blogs
        .OrderBy(b => b.BlogId)
        .First();
    db.Remove(blog);
    db.SaveChanges();
});

ListBlogs();


void ListBlogs()
{
    Console.WriteLine("List blogs");
    ExecuteDb(db =>
    {
        foreach (var blog in db.Blogs
            .OrderBy(b => b.BlogId))
        {
            Console.WriteLine("Found {0}: {1} ({2} posts)", blog.BlogId, blog.Url, blog.Posts.Count);
        }
    });
}

void ExecuteDb(Action<BloggingContext> action)
{
    using var scope = host.Services.CreateScope();
    using var db = scope.ServiceProvider.GetRequiredService<BloggingContext>();
    action(db);
}