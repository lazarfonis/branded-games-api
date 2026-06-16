using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

/// <summary>
/// Endpoints for managing customer game forms (branded game configurations).
/// </summary>
[Route("api/customer-games")]
[ApiController]
public class GameFormController : BaseController
{
    private readonly GameFormManager gameFormManager;

    /// <summary>Creates a new <see cref="GameFormController"/>.</summary>
    /// <param name="gameFormManager">The game form manager.</param>
    public GameFormController(GameFormManager gameFormManager)
    {
        this.gameFormManager = gameFormManager;
    }

    /// <summary>Gets all game forms.</summary>
    /// <returns>GameFormModels</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameFormModel>))]
    public async Task<IActionResult> GetGames()
    {
        var result = await gameFormManager.GetGames();
        return Ok(result);
    }

    /// <summary>Gets a single game form by its identifier.</summary>
    /// <param name="id">Game form identifier</param>
    /// <returns>GameFormModel</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameFormModel))]
    public async Task<IActionResult> GetGame(Guid id)
    {
        var result = await gameFormManager.GetGame(id);
        return Ok(result);
    }

    /// <summary>Creates a game form, including its uploaded files.</summary>
    /// <param name="model">The game configuration, sent as multipart form data.</param>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateGame([FromForm] GameFormCreateModel model)
    {
        model.Files = model.Files.Any() ? model.Files : Request.Form.Files.ToList();
        await gameFormManager.Create(model);
        return NoContent();
    }

    /// <summary>Deletes a game form.</summary>
    /// <param name="id">Game form identifier</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        await gameFormManager.DeleteGame(id);
        return NoContent();
    }
}
