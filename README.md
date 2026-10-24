# To-Do App (ASP.NET Core MVC + Dapper + Google OAuth)

## Quraşdırma
1. `Database/script.sql` faylını SQL Server / LocalDB-də işə salın (ToDoDb bazası yaranır).
2. Google Cloud Console -> APIs & Services -> Credentials -> OAuth client ID (Web application).
   Authorized redirect URI: `https://localhost:PORT/signin-google` (PORT launchSettings.json-dakı portdur).
3. ClientId və ClientSecret-i user-secrets ilə verin (GitHub-a push etməyin!):
   ```
   dotnet user-secrets init
   dotnet user-secrets set "Google:ClientId" "..."
   dotnet user-secrets set "Google:ClientSecret" "..."
   ```
4. `dotnet run`

## Qaydalar
- EF Core yoxdur, yalnız Dapper (DB-first: əvvəlcə SQL script).
- Daxili register yoxdur, yalnız Google login.
- Statuslar: Todo, In Progress, Done.
- In Progress və Done tasklar silinə bilməz (SQL-də `AND Status = 0` şərti ilə təmin olunur).
