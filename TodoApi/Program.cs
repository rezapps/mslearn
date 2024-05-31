using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


// MapGroup
var api = app.MapGroup("/todoitems");

// Endpoint Get All Todos
api.MapGet("/", async (TodoDb db) => 
    await db.Todos.ToListAsync());

// Endpoint Get Complete Todos
api.MapGet("/complete", async (TodoDb db) => 
    await db.Todos.Where(t => t.IsComplete).ToListAsync());


// Endpoint Get Todo by Id
api.MapGet("/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

// Endpoint Create Todo
api.MapPost("/", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    
    return Results.Created($"/todoitems/{todo.Id}", todo);
});


// Endpoint Update Todo
api.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});


// Endpoint Delete Todo
api.MapDelete("/{id}", async (int id, TodoDb db) => {
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

app.Run();
