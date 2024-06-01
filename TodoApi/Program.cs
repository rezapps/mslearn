using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

// var api = app.MapGroup("/todoitems");           		// MapGroup
RouteGroupBuilder api = app.MapGroup("/todoitems");		// MapGroup
api.MapGet("/", GetAllTodos);							// Endpoint Get All Todos
api.MapPost("/", CreateTodo);							// Endpoint Create Todo
api.MapGet("/{id}", GetTodoById);						// Endpoint Get Todo by Id
api.MapPut("/{id}", UpdateTodo);						// Endpoint Update Todo
api.MapDelete("/{id}", DeleteTodo);             		// Endpoint Delete Todo
api.MapGet("/complete", GetCompletedTodos);				// Endpoint Get Complete Todos

app.Run();


// ####### Updated methods to use DTO instead of Model ########

// static async Task<IResult> GetAllTodos(TodoDb db)
// {
// 	return TypedResults.Ok(await db.Todos.ToArrayAsync());
// }
static async Task<IResult> GetAllTodos(TodoDb db)
{
	return TypedResults.Ok(await db.Todos.Select(t => new TodoItemDTO(t)).ToArrayAsync());
}


// static async Task<IResult> GetCompletedTodos(TodoDb db)
// {
// 	return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToArrayAsync());
// }
static async Task<IResult> GetCompletedTodos(TodoDb db)
{
	return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(t => new TodoItemDTO(t)).ToArrayAsync());
}


// static async Task<IResult> GetTodoById(int id, TodoDb db)
// {
// 	return await db.Todos.FindAsync(id)
// 		is Todo todo
// 			? TypedResults.Ok(todo)
// 			: TypedResults.NotFound();
// }
static async Task<IResult> GetTodoById(int id, TodoDb db)
{
	return await db.Todos.FindAsync(id)
		is Todo todo
			? TypedResults.Ok(new TodoItemDTO(todo))
			: TypedResults.NotFound();
}



// static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
// {
// 	db.Todos.Add(todo);
// 	await db.SaveChangesAsync();
// 	return TypedResults.Created($"/todoitems/{todo.Id}", todo);
// }
static async Task<IResult> CreateTodo(TodoItemDTO todoDTO, TodoDb db)
{
	var newTask = new Todo
	{
		IsComplete = todoDTO.IsComplete,
		Name = todoDTO.Name
	};

	db.Todos.Add(newTask);
	await db.SaveChangesAsync();

	todoDTO = new TodoItemDTO(newTask);
	return TypedResults.Created($"/todoItems/{newTask.Id}", todoDTO);
}


// static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
// {
// 	var todo = await db.Todos.FindAsync(id);
// 	if (todo is null) return TypedResults.NotFound();
// 	todo.Name = inputTodo.Name;
// 	todo.IsComplete = inputTodo.IsComplete;
// 	await db.SaveChangesAsync();
// 	return Results.NoContent();
// }
static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoDTO, TodoDb db)
{
	var todoToUpdate = await db.Todos.FindAsync(id);

	if (todoToUpdate is null) return TypedResults.NotFound();

	todoToUpdate.Name = todoDTO.Name;
	todoToUpdate.IsComplete = todoDTO.IsComplete;

	await db.SaveChangesAsync();
	return TypedResults.NoContent();
}

// Delete method does not need to change as
// it is not using Todo
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
