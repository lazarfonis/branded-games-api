using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

/// <summary>
/// Endpoints for managing game types.
/// </summary>
[Route("api/game-types")]
[ApiController]
public class GameTypeController: BaseController
{
    private readonly GameTypeManager gameTypeManager;

    /// <summary>Creates a new <see cref="GameTypeController"/>.</summary>
    /// <param name="gameTypeManager">The game type manager.</param>
    public GameTypeController(GameTypeManager gameTypeManager)
    {
        this.gameTypeManager = gameTypeManager;
    }

    /// <summary>Gets all game types.</summary>
    /// <returns>GameTypeModels</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameTypeModel>))]
    public async Task<IActionResult> GetTypes()
    {
        var result = await gameTypeManager.GetTypes();
        return Ok(result);
    }

    /// <summary>Gets a single game type by its identifier.</summary>
    /// <param name="id">Game type identifier</param>
    /// <returns>GameTypeModel</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameTypeModel))]
    public async Task<IActionResult> GetType(Guid id)
    {
        var result = await gameTypeManager.GetType(id);
        return Ok(result);
    }

    /// <summary>Creates a game type.</summary>
    /// <param name="model">Game type to create</param>
    /// <returns>The created game type</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameTypeModel))]
    public async Task<IActionResult> CreateType([FromBody] GameTypeCreateModel model)
    {
        var result = await gameTypeManager.CreateType(model);
        return Ok(result);
    }

    /// <summary>Updates a game type.</summary>
    /// <param name="id">Game type identifier</param>
    /// <param name="model">New game type values</param>
    /// <returns>The updated game type</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameTypeModel))]
    public async Task<IActionResult> UpdateType(Guid id, [FromBody] GameTypeUpdateModel model)
    {
        var result = await gameTypeManager.UpdateType(id, model);
        return Ok(result);
    }

    /// <summary>Deletes a game type.</summary>
    /// <param name="id">Game type identifier</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteType(Guid id)
    {
        await gameTypeManager.DeleteType(id);
        return NoContent();
    }
}
