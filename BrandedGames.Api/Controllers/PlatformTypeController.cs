using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/platform-types")]
[ApiController]
public class PlatformTypeController: BaseController
{
    private readonly PlatformTypeManager platformTypeManager;

    public PlatformTypeController(PlatformTypeManager platformTypeManager)
    {
        this.platformTypeManager = platformTypeManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlatformTypeModel>))]
    public async Task<IActionResult> GetPlatforms()
    {
        var result = await platformTypeManager.GetPlatforms();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatformTypeModel))]
    public async Task<IActionResult> GetPlatform(Guid id)
    {
        var result = await platformTypeManager.GetPlatform(id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatformTypeModel))]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformTypeCreateModel model)
    {
        var result = await platformTypeManager.CreatePlatform(model);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatformTypeModel))]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformTypeUpdateModel model)
    {
        var result = await platformTypeManager.UpdatePlatform(id, model);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformTypeManager.DeletePlatform(id);
        return NoContent();
    }
}
