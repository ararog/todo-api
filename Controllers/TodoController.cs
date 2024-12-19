using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
  private readonly ILogger<UsersController> _logger;
  private readonly Services.TodoService _todoService;

  public TodoController(ILogger<UsersController> logger, Services.TodoService todoService)
  {
    _logger = logger;
    _todoService = todoService;
  }

  [Authorize(Roles = "Admin,User")]
  [HttpGet()]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IEnumerable<TodoItem>> GetTodo()
  {
    int id = int.Parse(ClaimTypes.NameIdentifier);
    _logger.LogDebug("Loading todo item");
    return await _todoService.GetAll(id);
  }

  [Authorize(Roles = "Admin,User")]
  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<TodoItem?> Get(string id)
  {
    string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    _logger.LogDebug($"Loading todo item of id {id}");
    var savedItem = await _todoService.Get(id);
    if (savedItem == null)
    {
      Response.StatusCode = StatusCodes.Status404NotFound;
      return null;
    }
    if (savedItem?.UserId != userId)
    {
      Response.StatusCode = StatusCodes.Status403Forbidden;
      return null;
    }
    return savedItem;
  }

  [Authorize(Roles = "Admin,User")]
  [HttpPost()]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Create([FromBody] TodoItem item)
  {
    string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (item.UserId != userId)
    {
      return Forbid();
    }
    _logger.LogDebug("Creating todo item");
    await _todoService.Create(item);
    return Ok();
  }

  [Authorize(Roles = "Admin,User")]
  [HttpPut("{id}")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Update(string id, [FromBody] TodoItem item)
  {
    string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var savedItem = await _todoService.Get(id);
    if (savedItem == null)
    {
      return NotFound();
    }
    if (savedItem?.UserId == userId)
    {
      return Forbid();
    }

    _logger.LogDebug($"Updating todo item of id {id}");
    await _todoService.Update(item);
    return NoContent();
  }

  [Authorize(Roles = "Admin,User")]
  [HttpDelete("{id}")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Delete(string id)
  {
    string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var savedItem = await _todoService.Get(id);
    if (savedItem == null)
    {
      return NotFound();
    }
    if (savedItem?.UserId == userId)
    {
      return Forbid();
    }

    _logger.LogDebug($"Deleting user of id {id}");
    await _todoService.Remove(id);
    return NoContent();
  }
}