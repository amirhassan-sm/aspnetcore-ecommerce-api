using Application.Contrast.Services;
using Application.DTOs.ProducMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Security;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMediaController : ControllerBase
    {
        private readonly IProductMediaService service;

        public ProductMediaController(IProductMediaService service)
        {
            this.service = service;
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost("AddNewMedia")]
        public async Task<IActionResult> AddNewMedia([FromForm] ProductMediaEntryDto media)
        {
            var model = new ProductMediaModel
            {
                ConfirmByUserName = User.FindFirst("userName")?.Value ?? "",
                FileName = media.FileStream?.FileName ?? "",
                fileSize = media.FileStream?.Length ?? 0,
                IsConfirmed = media.IsConfirmed,
                MediaTypeId = media.MediaTypeId,
                ProductId = media.ProductId,
                UploadingUserName = User.FindFirst("userName")?.Value ?? "",
                FileStream = media.FileStream!.OpenReadStream(),
            };

            var result = await service.AddMediaAsync(model);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetMedia/{id}")]
        public async Task<IActionResult> GetMediaById(int id)
        {
            var result = await service.GetByIdAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllProductMediaByproductId/{productId}")]
        public async Task<IActionResult> GetAllProductMedia(int productId)
        {
            var result = await service.GetAllMediaByProductIdAsync(productId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetImagesByProductId/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var result = await service.GetImagesByProductIdAsync(productId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetVideosByProductId/{productId}")]
        public async Task<IActionResult> GetVideosByProductId(int productId)
        {
            var result = await service.GetVideosByProductIdAsync(productId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{ProductMediaId}")]
        public async Task<IActionResult> RemoveMedia(int ProductMediaId)
        {
            var result = await service.RemoveAsync(ProductMediaId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
