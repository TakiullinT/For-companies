using Microsoft.AspNetCore.Mvc;
using Timescale.Api.Application.DTOs;
using Timescale.Api.Application.Interfaces;

namespace Timescale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _resultService;

    public ResultsController(IResultService resultService)
    {
        _resultService = resultService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResultResponseDTO>>> GetResults([FromQuery] ResultFilterDTO filter)
    {
        var results = await _resultService.GetFilteredResultsAsync(filter);
        return Ok(results);
    }
}