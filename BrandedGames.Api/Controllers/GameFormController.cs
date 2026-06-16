using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/customer-games")]
[ApiController]
public class GameFormController : BaseController
{
    private readonly GameFormManager gameFormManager;

    public GameFormController(GameFormManager gameFormManager)
    {
        this.gameFormManager = gameFormManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameFormModel>))]
    public async Task<IActionResult> GetGames()
    {
        var result = await gameFormManager.GetGames();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameFormModel))]
    public async Task<IActionResult> GetGame(Guid id)
    {
        var result = await gameFormManager.GetGame(id);
        return Ok(result);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateGame([FromForm] GameFormCreateModel model)
    {
        model.Files = model.Files.Any() ? model.Files : Request.Form.Files.ToList();
        await gameFormManager.Create(model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        await gameFormManager.DeleteGame(id);
        return NoContent();
    }
}
