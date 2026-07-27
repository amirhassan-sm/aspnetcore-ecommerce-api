using Application.Contrast.Services;
using Application.DTOs.Feature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Security;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureApplicationServiceContract featureApp;

        public FeatureController(IFeatureApplicationServiceContract featureApp)
        {
            this.featureApp = featureApp;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await featureApp.Get(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] FeatureSearchModel sm)
        {
            var result = await featureApp.Search(sm);
            return Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("add")]
        public async Task<IActionResult> AddFeature([FromBody] FeatureAddModel model)
        {
            var result = await featureApp.AddNew(model);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await featureApp.Remove(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("UpdateFeature")]
        public async Task<IActionResult> UpdateFeature(FeatureModel model)
        {
            var result = await featureApp.Update(model);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
