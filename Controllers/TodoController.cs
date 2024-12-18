using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using UsersService.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
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
  public async Task<IEnumerable<TodoItem>> Get()
  {
    _logger.LogDebug("Loading todo item");
    return await _todoService.Get();
  }

  [Authorize(Roles = "Admin,User")]
  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<TodoItem?> Get(string id)
  {
    _logger.LogDebug($"Loading todo item of id {id}");
    return await _todoService.Get(id);
  }

  [Authorize(Roles = "Admin,User")]
  [HttpPost()]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create([FromBody] TodoItem item)
  {
    _logger.LogDebug("Creating todo item");
    await _todoService.Create(item);
    return Ok();
  }

  [Authorize(Roles = "Admin,User")]
  [HttpPut("{id}")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Update(string id, [FromBody] TodoItem item)
  {
    _logger.LogDebug($"Updating todo item of id {id}");
    await _todoService.Update(item);
    return NoContent();
  }

  [Authorize(Roles = "Admin,User")]
  [HttpDelete("{id}")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Delete(string id)
  {
    _logger.LogDebug($"Deleting user of id {id}");
    await _todoService.Remove(id);
    return NoContent();
  }
}