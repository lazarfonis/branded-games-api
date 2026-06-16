using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

[Route("api/features")]
[ApiController]
public class FeatureController: BaseController
{
    private readonly FeatureManager featureManager;

    public FeatureController(FeatureManager featureManager)
    {
        this.featureManager = featureManager;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FeatureModel>))]
    public async Task<IActionResult> GetFeatures()
    {
        var result = await featureManager.GetFeatures();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeatureModel))]
    public async Task<IActionResult> GetFeature(Guid id)
    {
        var result = await featureManager.GetFeature(id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeatureModel))]
    public async Task<IActionResult> CreateFeature([FromBody] FeatureCreateModel model)
    {
        var result = await featureManager.CreateFeature(model);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeatureModel))]
    public async Task<IActionResult> UpdateFeature(Guid id, [FromBody] FeatureUpdateModel model)
    {
        var result = await featureManager.UpdateFeature(id, model);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        await featureManager.DeleteFeature(id);
        return NoContent();
    }
}
