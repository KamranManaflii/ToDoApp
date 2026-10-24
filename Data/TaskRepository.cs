using AspNetCoreMvc.Models;
using Dapper;

namespace AspNetCoreMvc.Data;

public class TaskRepository
{
    private readonly DbConnectionFactory _factory;
    public TaskRepository(DbConnectionFactory factory) => _factory = factory;

    public async Task<List<TaskItem>> GetByUserAsync(int userId)
    {
        using var db = _factory.Create();
        var tasks = await db.QueryAsync<TaskItem>(
            "SELECT * FROM Tasks WHERE UserId = @userId ORDER BY CreatedAt DESC",
            new { userId });
        return tasks.ToList();
    }

    public async Task AddAsync(int userId, string title, string? description)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "INSERT INTO Tasks (UserId, Title, Description, Status) VALUES (@userId, @title, @description, 0)",
            new { userId, title, description });
    }

    // Status dəyişmək (yalnız öz taskı)
    public async Task<bool> ChangeStatusAsync(int id, int userId, TaskState status)
    {
        using var db = _factory.Create();
        var rows = await db.ExecuteAsync(
            "UPDATE Tasks SET Status = @status WHERE Id = @id AND UserId = @userId",
            new { id, userId, status = (byte)status });
        return rows > 0;
    }

    // Qayda: yalnız Todo (Status = 0) olan task silinə bilər.
    // In Progress və Done tasklar şərtə düşmür, ona görə silinmir.
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        using var db = _factory.Create();
        var rows = await db.ExecuteAsync(
            "DELETE FROM Tasks WHERE Id = @id AND UserId = @userId AND Status = 0",
            new { id, userId });
        return rows > 0;
    }
}
