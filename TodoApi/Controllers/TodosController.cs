using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
	private readonly TodoDb _context;

	public TodosController(TodoDb context)
	{
		_context = context;
	}

	// GET: api/todos
	[HttpGet]
	public async Task<IResult> GetAllTodos(TodoDb _context)
	{
		return TypedResults.Ok(await _context.Todos.Select(t => new TodoItemDTO(t)).ToArrayAsync());
	}

	// GET: api/todos/complete
	[HttpGet("complete")]
	public async Task<IResult> GetCompletedTodos(TodoDb _context)
	{
		return TypedResults.Ok(await _context.Todos.Where(t => t.IsComplete).Select(t => new TodoItemDTO(t)).ToArrayAsync());
	}

	// GET: api/todos/5
	[HttpGet("{id}")]
	public async Task<IResult> GetTodoById(int id, TodoDb _context)
	{
		return await _context.Todos.FindAsync(id)
			is Todo todo
				? TypedResults.Ok(new TodoItemDTO(todo))
				: TypedResults.NotFound();
	}

	// POST: api/todos
	[HttpPost]
	public async Task<IResult> CreateTodo(TodoItemDTO todoDTO, TodoDb _context)
	{
		var newTask = new Todo
		{
			IsComplete = todoDTO.IsComplete,
			Name = todoDTO.Name
		};

		_context.Add(newTask);
		await _context.SaveChangesAsync();

		todoDTO = new TodoItemDTO(newTask);
		return TypedResults.Created($"/todos/{newTask.Id}", todoDTO);
	}

	// PUT: api/todos/5
	[HttpPut("{id}")]
	public async Task<IResult> UpdateTodo(int id, TodoItemDTO todoDTO, TodoDb _context)
	{
		var todoToUpdate = await _context.Todos.FindAsync(id);

		if (todoToUpdate is null) return TypedResults.NotFound();

		todoToUpdate.Name = todoDTO.Name;
		todoToUpdate.IsComplete = todoDTO.IsComplete;

		await _context.SaveChangesAsync();
		return TypedResults.NoContent();
	}

	// DELETE: api/todos/5
	[HttpDelete("{id}")]
	public async Task<IResult> DeleteTodo(int id, TodoDb _context)
	{
		if (await _context.Todos.FindAsync(id) is Todo todo)
		{
			_context.Todos.Remove(todo);
			await _context.SaveChangesAsync();
			return TypedResults.Ok(todo);
		}
		return TypedResults.NotFound();
	}
}
