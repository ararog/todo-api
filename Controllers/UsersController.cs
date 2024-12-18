using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
  private readonly ILogger<UsersController> _logger;
  private readonly Services.UsersService _usersService;

  public UsersController(ILogger<UsersController> logger, Services.UsersService usersService)
  {
    _logger = logger;
    _usersService = usersService;
  }

  [Authorize(Roles = "Admin")]
  [HttpGet()]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IEnumerable<User>> Get()
  {
    _logger.LogDebug("Loading users");
    return await _usersService.Get();
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<User?> Get(string id)
  {
    _logger.LogDebug($"Loading user of id {id}");
    return await _usersService.Get(id);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost()]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create([FromBody] User user)
  {
    _logger.LogDebug("Creating user");
    await _usersService.Create(user);
    return Ok();
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("{id}")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Update(string id, [FromBody] User user)
  {
    _logger.LogDebug($"Updating user of id {id}");
    await _usersService.Update(user);
    return NoContent();
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{id}")]
  [Consumes(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Delete(string id)
  {
    _logger.LogDebug($"Deleting user of id {id}");
    await _usersService.Remove(id);
    return NoContent();
  }
}
