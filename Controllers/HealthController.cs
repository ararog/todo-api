using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace TodoApi.Controllers;

[ApiController]
[EnableRateLimiting("sliding")]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{

  public HealthController()
  {
  }

  [HttpGet()]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IActionResult Check()
  {
    return Ok();
  }

}