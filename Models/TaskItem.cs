namespace AspNetCoreMvc.Models;

public enum TaskState : byte
{
    Todo = 0,
    InProgress = 1,
    Done = 2
}

public class TaskItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public TaskState Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
