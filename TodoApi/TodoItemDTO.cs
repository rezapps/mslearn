namespace TodoApi;

public class TodoItemDTO
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public bool IsComplete { get; set; }


	// Empty constructor for serialization and deserialization
	// this lets the deserializer create an instance of TodoItemDTO
	// and then sets the properties from the deserialized JSON
	public TodoItemDTO() { }
	public TodoItemDTO(Todo todoItem)
	{
		(Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
	}
}
