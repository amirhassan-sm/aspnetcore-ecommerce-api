using Application.Contrast.Services;
using Application.DTOs.ProducMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProductMediaController : ControllerBase
    {
        private readonly IProductMediaService service;


        

        public ProductMediaController(IProductMediaService service)
        {
            this.service = service;

        }

        [HttpPost("AddNewMedia")]

        public async Task<IActionResult> AddNewMedia([FromForm] ProductMediaEntryDto media)
        {
            var model = new ProductMediaModel
            {
                ConfirmByUserName = User.FindFirst("userName").Value ?? "",
                FileName = media.FileStream?.FileName ??"",
                fileSize = media.FileStream.Length,
                IsConfirmed = media.IsConfirmed,
                MediaTypeId = media.MediaTypeId,
                ProductId = media.ProductId,

                UploadingUserName = User.FindFirst("userName").Value ?? "",
                FileStream = media.FileStream.OpenReadStream(),





            };


            var result = await service.AddMediaAsync(model);
            if (!result.Success)
            {
                return BadRequest(result);


            }
            return Ok(result);



        }

        [HttpGet("GetMedia/{id}")]
        public async Task<IActionResult> GetMediaById(int id)
        {
            var result = await service.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);

        }
        [HttpGet("GetAllProductMediaByproductId/{productId}")]
        public async Task<IActionResult> GetAllProductMedia(int productId)
        {
            var result = await service.GetAllMediaByProductIdAsync(productId);
            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);




        }
        [HttpGet("GetImagesByProductId/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var result = await service.GetImagesByProductIdAsync(productId);
            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);




        }
        [HttpGet("GetVideosByProductId/{productId}")]
        public async Task<IActionResult> GetVideosByProductId(int productId)
        {
            var result = await service.GetVideosByProductIdAsync(productId);
            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);




        }

        [HttpDelete("/{ProductMediaId}")]
        public async Task<IActionResult> RemoveMedia(int ProductMediaId)
        {
            var result = await service.RemoveAsync(ProductMediaId);
            if (!result.Success)
            {
                return BadRequest(result);


            }

            return Ok(result);
        }






    }
}
