using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;



namespace Shopping.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(Roles ="admin")]
    public class CategoryAdminManagementController : ControllerBase
    {
        private readonly ICategoryApplicationServiceContract CatApp;
        public CategoryAdminManagementController(ICategoryApplicationServiceContract CatApp)
        {
            this.CatApp = CatApp;

        }




        [HttpPost("AddNewCategory")]
        public async Task<IActionResult> AddNewCategory( CategoryAddModel cat)
        {
            
           

            var op = await CatApp.AddNew(cat);
            if (op.Success == true)
            {
                
                return  StatusCode(StatusCodes.Status202Accepted,op);
            }
            return BadRequest(op);



        }



        [HttpGet("GetCategory/{CategoryId}")]
        public async Task<IActionResult> GetCategory(int CategoryId)
        {

            var op = await CatApp.Get(CategoryId);
            if (op.Success == true)
            {
                return Ok(op);
            }

            return BadRequest(op);
        }

        [HttpPost("AssignFeatureToCategory")]

        public async Task<IActionResult> AssignFeatureToCategory(int CategoryId , int FeatureId)
        { 
          var op =   await CatApp.AssignFeatureToCategory(CategoryId , FeatureId);
            if (op.Success == true)
            {
                return Ok(op);
                
            }
            return BadRequest(op);

        }

        [HttpDelete("RemoveCategory")]
        public async Task<IActionResult> RemoveCategory( int CategoryId) 
        {
            var op = await CatApp.Remove(CategoryId);
            if (op.Success == true)
            {
                return Ok(op);
                
            }
            return BadRequest(op);

        }
        [HttpGet("SearchCategory")]
        public async Task<IActionResult> Search([FromQuery]CategorySearchModel sm) 
        {
            try
            {
                var catItems = await CatApp.Search(sm);
                return Ok(catItems);
            }
            catch (Exception ex) { 
                return BadRequest(ex);
            
           
            }



        }
        [HttpDelete("RemoveFeatureFromCategory")]
        public async Task<IActionResult> RemoveFeatureFromCategory([FromRoute]int FeatureId, int CategoryId)
        {
            var op = await CatApp.RemoveFeatureToCategory(CategoryId, FeatureId);
            if (op.Success == true) { 
            return Ok(op);
            }

            return BadRequest(op);
        
        }


        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory( [FromBody]CategoryUpdateEntryDto model) 
        { 
            try
            {
                var catModel = new CategoryUpdateModel {
                    CategoryId = model.CategoryId,
                    CategoryDescription = model.CategoryDescription,
                    CategoryName = model.CategoryName,
                    ParentId = model.ParentId,
                    Slug = model.Slug,
                
                
                };
                var op = await CatApp.Update(catModel);
                if (op.Success == true)
                {
                    return Ok(op);

                }

                return BadRequest(op);

            }
            catch (Exception ex) { 
            
            return BadRequest(OperationResult.ToFail("Update failed",new List<string> { "something happens! please call to our contacts "} ));
            
            }

        }

    }
}
