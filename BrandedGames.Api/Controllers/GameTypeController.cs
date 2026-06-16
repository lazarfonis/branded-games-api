using BrandedGames.Common.Models;
using BrandedGames.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/game-types")]
[ApiController]
public class GameTypeController: BaseController
{
    private readonly GameTypeManager gameTypeManager;

    public GameTypeController(GameTypeManager gameTypeManager)
    {
        this.gameTypeManager = gameTypeManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameTypeModel>))]
    public async Task<IActionResult> GetTypes()
    {
        var result = await gameTypeManager.GetTypes();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameTypeModel))]
    public async Task<IActionResult> GetType(Guid id)
    {
        var result = await gameTypeManager.GetType(id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameTypeModel))]
    public async Task<IActionResult> CreateType([FromBody] GameTypeCreateModel model)
    {
        var result = await gameTypeManager.CreateType(model);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameTypeModel))]
    public async Task<IActionResult> UpdateType(Guid id, [FromBody] GameTypeUpdateModel model)
    {
        var result = await gameTypeManager.UpdateType(id, model);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteType(Guid id)
    {
        await gameTypeManager.DeleteType(id);
        return NoContent();
    }
}
