using Application.Contrast.Services;
using Application.DTOs.Product;
using Application.DTOs.ProductFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductApplicationServiceContract productApp;
        public ProductController(IProductApplicationServiceContract productApp)
        {
            this.productApp = productApp;

        }
        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct([FromQuery] ProductSearchModel model)
        {
            var result = await productApp.SearchProductAsync(model);
            return Ok(result);

        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody]ProductAddModel model)
        {
            var result = await productApp.AddNew(model);
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) { 
            var result =  await productApp.Get(id);
            if (result.Success)
            {
                return Ok(result);
                
            }
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var result = await productApp.Remove(id);
            if (result.Success)
            {
                return Ok(result);
                
            }
            return BadRequest(result);
        }

        [HttpPut("SetSeoModel")]

        public async Task<IActionResult> SetSeoModel([FromBody]ProductSeoModel model)
        {
            var result = await productApp.setSeoAsync(model);
            if (result.Success)
            {
                return Ok(result);
                
            }
            return BadRequest(result);

        }

        [HttpPut("Update")]

        public async Task<IActionResult> Update([FromBody]ProductUpdateModel model) { 
        var result = await productApp.Update(model);

            if (result.Success)
            {
                return Ok(result);
                
            }
            return BadRequest(result);

        }
        [HttpGet("GetProductFeatures")]
        public async Task<IActionResult> GetProductFeatures(int id)
        {
        var result = await productApp.GetAllProductFeatures(id);
            if (!result.Success)
            {
                return BadRequest(result);

                
            }
            return Ok(result);
        }
        [HttpPost("assigneFeatureToProduct/{ProductId}/{FeatureId}")]
        public async Task<IActionResult> assigneFeatureToProduct([FromRoute]int ProductId,int FeatureId)
        {
            var result = await productApp.AssignFeatureToProductAsync(FeatureId:FeatureId,ProductId:ProductId);
            if (!result.Success)
            {
                return BadRequest(result);
                
            }
            return Ok(result);


        }

        [HttpDelete("RemoveFeatureFromProduct/{ProductId}/{FeatureId}")]
        public async Task<IActionResult> RemoveFeatureFromProduct([FromRoute] int ProductId, int FeatureId)
        {
            var result = await productApp.RemoveFeaturesFromProductAsync(FeatureId: FeatureId, ProductId: ProductId);
            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);


        }

        [HttpPut("SetFeaturesValue")]
        public async Task<IActionResult> SetFeaturesValue([FromBody]SetProductFeatureModel model)
        {
            var result = await productApp.SetFeatureValues(model);
            if (!result.Success)
            {
                return BadRequest(result);
                
            }
            return Ok(result);

        }




    }
}
