using AspNetCoreMvc.Models;
using Dapper;

namespace AspNetCoreMvc.Data;

public class UserRepository
{
    private readonly DbConnectionFactory _factory;
    public UserRepository(DbConnectionFactory factory) => _factory = factory;

    // Google ilə ilk dəfə girəndə user yaradılır, sonra mövcud user qaytarılır
    public async Task<User> GetOrCreateAsync(string googleId, string email, string? fullName)
    {
        using var db = _factory.Create();

        var user = await db.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE GoogleId = @googleId", new { googleId });

        if (user != null) return user;

        var id = await db.ExecuteScalarAsync<int>(
            @"INSERT INTO Users (GoogleId, Email, FullName)
              VALUES (@googleId, @email, @fullName);
              SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { googleId, email, fullName });

        return await db.QueryFirstAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
    }
}
