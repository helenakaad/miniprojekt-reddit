using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Sætter CORS så API'en kan bruges fra andre domæner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Tilføj DbContext factory som service.
builder.Services.AddDbContext<PostsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

var app = builder.Build();

// Seed data hvis nødvendigt.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


// DataService fås via "Dependency Injection" (DI)
app.MapGet("/", (DataService service) =>
{
    return new { message = "Forside til tråde :)" };
});

app.MapGet("api/posts", (DataService service) =>
{
    return service.GetPosts().Select(a => new { a.PostsId, a.Header, a.Text, a.Name, a.Upvotes, a.DownVotes, a.Comments, a.DateTime });
});


app.MapGet("api/posts/{id}", (DataService service, int id) =>
{
    return service.GetSinglePost(id);
});

app.MapPost("api/posts/newPost", (DataService service, NewPostData data) =>
{
    string result = service.CreatePost(data.name, data.text, data.header);
    return new { message = result };
});

app.MapPost("api/posts/{id}/newComment", (DataService service, NewCommentData data, int id) =>
{
    Console.WriteLine(data.name, id);
    string result = service.createComment(data.name, data.text, id);
    return new { message = result };
});

app.MapPut("api/posts/{id}/upvote", (DataService service, int id) =>
{
    string result = service.upvotePost(id);
    return new { message = result };
});

app.MapPut("api/posts/{id}/downvote", (DataService service, int id) =>
{
    string result = service.downvotePost(id);
    return new { message = result };
});

app.MapPut("api/posts/{id}/upvoteC", (DataService service, int id) =>
{
    string result = service.UpvoteComment(id);
    return new { message = result };
});

app.MapPut("api/posts/{id}/downvoteC", (DataService service, int id) =>
{
    string result = service.DownvoteComment(id);
    return new { message = result };
});

app.Run();

record NewPostData(string name, string text, string header);
record NewCommentData(string name, string text);