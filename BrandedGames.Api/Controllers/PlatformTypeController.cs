using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

/// <summary>
/// Endpoints for managing platform types.
/// </summary>
[Route("api/platform-types")]
[ApiController]
public class PlatformTypeController: BaseController
{
    private readonly PlatformTypeManager platformTypeManager;

    /// <summary>Creates a new <see cref="PlatformTypeController"/>.</summary>
    /// <param name="platformTypeManager">The platform type manager.</param>
    public PlatformTypeController(PlatformTypeManager platformTypeManager)
    {
        this.platformTypeManager = platformTypeManager;
    }

    /// <summary>Gets all platform types.</summary>
    /// <returns>PlatformTypeModels</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlatformTypeModel>))]
    public async Task<IActionResult> GetPlatforms()
    {
        var result = await platformTypeManager.GetPlatforms();
        return Ok(result);
    }

    /// <summary>Gets a single platform type by its identifier.</summary>
    /// <param name="id">Platform type identifier</param>
    /// <returns>PlatformTypeModel</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatformTypeModel))]
    public async Task<IActionResult> GetPlatform(Guid id)
    {
        var result = await platformTypeManager.GetPlatform(id);
        return Ok(result);
    }

    /// <summary>Creates a platform type.</summary>
    /// <param name="model">Platform type to create</param>
    /// <returns>The created platform type</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatformTypeModel))]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformTypeCreateModel model)
    {
        var result = await platformTypeManager.CreatePlatform(model);
        return Ok(result);
    }

    /// <summary>Updates a platform type.</summary>
    /// <param name="id">Platform type identifier</param>
    /// <param name="model">New platform type values</param>
    /// <returns>The updated platform type</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlatformTypeModel))]
    public async Task<IActionResult> UpdatePlatform(Guid id, [FromBody] PlatformTypeUpdateModel model)
    {
        var result = await platformTypeManager.UpdatePlatform(id, model);
        return Ok(result);
    }

    /// <summary>Deletes a platform type.</summary>
    /// <param name="id">Platform type identifier</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        await platformTypeManager.DeletePlatform(id);
        return NoContent();
    }
}
