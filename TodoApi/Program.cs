using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


// MapGroup
var api = app.MapGroup("/todoitems");
api.MapGet("/", GetAllTodos);					// Endpoint Get All Todos
api.MapPost("/", CreateTodo);					// Endpoint Create Todo
api.MapGet("/complete", GetCompletedTodos);		// Endpoint Get Complete Todos
api.MapGet("/{id}", GetTodoById);				// Endpoint Get Todo by Id
api.MapPut("/{id}", UpdateTodo);				// Endpoint Update Todo
api.MapDelete("/{id}", DeleteTodo);             // Endpoint Delete Todo

app.Run();

// api.MapGet("/", async (TodoDb db) =>
//     await db.Todos.ToListAsync());
static async Task<IResult> GetAllTodos(TodoDb db)
{
	return TypedResults.Ok(await db.Todos.ToArrayAsync());
}

// api.MapGet("/complete", async (TodoDb db) =>
//     await db.Todos.Where(t => t.IsComplete).ToListAsync());
static async Task<IResult> GetCompletedTodos(TodoDb db)
{
	return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToArrayAsync());
}

// api.MapGet("/{id}", async (int id, TodoDb db) =>
//     await db.Todos.FindAsync(id)
//         is Todo todo
//             ? Results.Ok(todo)
//             : Results.NotFound());
static async Task<IResult> GetTodoById(int id, TodoDb db)
{
	return await db.Todos.FindAsync(id)
		is Todo todo
			? TypedResults.Ok(todo)
			: TypedResults.NotFound();
}

// api.MapPost("/", async (Todo todo, TodoDb db) =>
// {
//     db.Todos.Add(todo);
//     await db.SaveChangesAsync();

//     return Results.Created($"/todoitems/{todo.Id}", todo);
// });
static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
{
	db.Todos.Add(todo);
	await db.SaveChangesAsync();
	return TypedResults.Created($"/todoitems/{todo.Id}", todo);
}

// api.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return Results.NotFound();

//     todo.Name = inputTodo.Name;
//     todo.IsComplete = inputTodo.IsComplete;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });
static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
{
	var todo = await db.Todos.FindAsync(id);
	if (todo is null) return TypedResults.NotFound();
	todo.Name = inputTodo.Name;
	todo.IsComplete = inputTodo.IsComplete;
	await db.SaveChangesAsync();
	return Results.NoContent();
}

// api.MapDelete("/{id}", async (int id, TodoDb db) => {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return Results.Ok(todo);
//     }
//     return Results.NotFound();
// });
static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
	if (await db.Todos.FindAsync(id) is Todo todo)
	{
		db.Todos.Remove(todo);
		await db.SaveChangesAsync();
		return TypedResults.Ok(todo);
	}
	return TypedResults.NotFound();
}
