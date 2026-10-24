using System.Security.Claims;
using AspNetCoreMvc.Data;
using AspNetCoreMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc.Controllers;

[Authorize]
public class TasksController : Controller
{
    private readonly TaskRepository _tasks;
    public TasksController(TaskRepository tasks) => _tasks = tasks;

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> Index()
    {
        var tasks = await _tasks.GetByUserAsync(CurrentUserId);
        return View(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            TempData["Error"] = "Task başlığı boş ola bilməz.";
            return RedirectToAction(nameof(Index));
        }
        await _tasks.AddAsync(CurrentUserId, title.Trim(), description?.Trim());
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, TaskState status)
    {
        await _tasks.ChangeStatusAsync(id, CurrentUserId, status);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _tasks.DeleteAsync(id, CurrentUserId);
        if (!deleted)
            TempData["Error"] = "Yalnız 'Todo' statusunda olan task silinə bilər.";
        return RedirectToAction(nameof(Index));
    }
}
