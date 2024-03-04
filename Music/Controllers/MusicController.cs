//using ACSystem.Api.Controllers;
using Api.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Music.Controllers;

//[ApiVersion("1", Deprecated = false)]
[Route("[controller]")]

public class MusicController : BaseController
{
    [HttpGet]
    public IActionResult GetLiatMusic()
    {
        return Ok();
    }
    [HttpGet]
    public IActionResult GetMusic(string id)
    {
        return Ok(new { id });
    }

}