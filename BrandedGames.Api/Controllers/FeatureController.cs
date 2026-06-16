using BrandedGames.Common.Models;
using BrandedGames.Core;
using BrandedGames.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrandedGames.Api.Controllers;

/// <summary>
/// Endpoints for managing game features.
/// </summary>
[Route("api/features")]
[ApiController]
public class FeatureController: BaseController
{
    private readonly FeatureManager featureManager;

    /// <summary>Creates a new <see cref="FeatureController"/>.</summary>
    /// <param name="featureManager">The feature manager.</param>
    public FeatureController(FeatureManager featureManager)
    {
        this.featureManager = featureManager;
    }

    /// <summary>Gets all features.</summary>
    /// <returns>FeatureModels</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FeatureModel>))]
    public async Task<IActionResult> GetFeatures()
    {
        var result = await featureManager.GetFeatures();
        return Ok(result);
    }

    /// <summary>Gets a single feature by its identifier.</summary>
    /// <param name="id">Feature identifier</param>
    /// <returns>FeatureModel</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeatureModel))]
    public async Task<IActionResult> GetFeature(Guid id)
    {
        var result = await featureManager.GetFeature(id);
        return Ok(result);
    }

    /// <summary>Creates a feature.</summary>
    /// <param name="model">Feature to create</param>
    /// <returns>The created feature</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeatureModel))]
    public async Task<IActionResult> CreateFeature([FromBody] FeatureCreateModel model)
    {
        var result = await featureManager.CreateFeature(model);
        return Ok(result);
    }

    /// <summary>Updates a feature.</summary>
    /// <param name="id">Feature identifier</param>
    /// <param name="model">New feature values</param>
    /// <returns>The updated feature</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeatureModel))]
    public async Task<IActionResult> UpdateFeature(Guid id, [FromBody] FeatureUpdateModel model)
    {
        var result = await featureManager.UpdateFeature(id, model);
        return Ok(result);
    }

    /// <summary>Deletes a feature.</summary>
    /// <param name="id">Feature identifier</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        await featureManager.DeleteFeature(id);
        return NoContent();
    }
}
