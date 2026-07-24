using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs;
using Catalogue.Domain.Models;
using Catalogue.DomainServiceContract.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.implementation
{
    public class ProductMediaService : IProductMediaService
    {
        private readonly IProductRepository ProductRepo;
        private readonly IProductMediaRepository mediaRepo;
        private readonly IFileStorage fileStorage;
        private readonly ILogger<ProductMediaService> logger;
        public ProductMediaService(IProductRepository ProductRepo, IProductMediaRepository mediaRepo, IFileStorage fileStorage, ILogger<ProductMediaService> logger)
        {
            this.ProductRepo = ProductRepo;
            this.mediaRepo = mediaRepo;
            this.fileStorage = fileStorage;
            this.logger = logger;

        }
        private OperationResult ValidateMedia(ProductMediaModel model)
        {
            // 1. File name
            if (string.IsNullOrWhiteSpace(model.FileName))
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "File name is required." },
                    "FileName_Required");
            }

            // 2. Stream
            if (model.FileStream == null || !model.FileStream.CanRead)
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "Invalid file." },
                    "Invalid_File");
            }

            // 3. File size
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB

            if (model.fileSize <= 0)
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "File is empty." },
                    "Empty_File");
            }

            if (model.fileSize > maxFileSize)
            {
                return OperationResult.ToFail(
                    "Upload failed",
                    new List<string> { "Maximum allowed size is 5 MB." },
                    "File_Too_Large");
            }

            // 4. Extension
            var extension = Path.GetExtension(model.FileName)
                   .ToLowerInvariant();

            if (model.MediaTypeId == 1)
            {



                var allowedExtensions = new[]
                {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        
    };


               

                if (!allowedExtensions.Contains(extension))
                {
                    return OperationResult.ToFail(
                        "Upload failed",
                        new List<string> { "Only JPG, JPEG, PNG and WEBP images are allowed." },
                        "Invalid_File_Extension");
                }
            }
            if (model.MediaTypeId == 2)
            {
                var allowedExtensions = new[]
               {
    ".mp4", ".mov", ".avi", ".mkv", ".webm"
};
                

                if (!allowedExtensions.Contains(extension))
                {
                    return OperationResult.ToFail(
                        "Upload failed",
                        new List<string> { "Only mp4, mov, mkv and webm images are allowed." },
                        "Invalid_File_Extension");
                }




            }
            if (model.MediaTypeId!= 1&& model.MediaTypeId!= 2)
            {
                return OperationResult.ToFail(
                        "Upload failed",
                        new List<string> { "invalid media type" },
                        "Invalid_MediaType");

            }

            return OperationResult.ToSuccess("product media model is valid");
        }
        private ProductMedia MediaModelMapToMedia(ProductMediaModel media)
        {
            return new ProductMedia
            {
                ConfirmByUserName = media.ConfirmByUserName,
                DisplayOrder = media.DisplayOrder,
                fileSize = media.fileSize,
                IsConfirmed = media.IsConfirmed,
                MediaTypeId = media.MediaTypeId,
                ProductId = media.ProductId,
                UploadingUserName = media.UploadingUserName,
                Url = media.Url,



            };


        }
        //TODO  add a display order
        public async Task<OperationResult> AddMediaAsync(ProductMediaModel media)
        {

            if (!await ProductRepo.IsPrductExist(media.ProductId))
            {
                return OperationResult.ToFail("failed to add media", new List<string> { "this product does not exist" }, "Product_Not_exist");

            }
            if (media.MediaTypeId != 1 && media.MediaTypeId != 2)
            {
                return OperationResult.ToFail("failed to add media ", new List<string> { "media type id can only be 1for images  or 2 for videos ," });

            }

            var isValid = ValidateMedia(media);
            if (!isValid.Success)
            {
                return isValid;

            }
            if (!await mediaRepo.IsMediaTypeExist(media.MediaTypeId))
            {
                return OperationResult.ToFail("Failed to add media", new List<string> { " this media type id does not exist" }, "MediaType_NotExist");

            }
            string? MediaLocation = "";

            try
            {


                MediaLocation = await fileStorage.SaveAsync(media.FileStream, media.FileName);
                media.Url = MediaLocation;
                await mediaRepo.AddAsync(MediaModelMapToMedia(media));
                return OperationResult.ToSuccess("Mdeia added successfully");
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(MediaLocation))
                {
                    await fileStorage.DeleteAsync(MediaLocation);

                }

                logger.LogError(
ex,
"Failed to add media to product {ProductId}",
media.ProductId);

                return OperationResult.ToFail("Failed to addd product media", new List<string> { "an unexpected error occured" }, "Exception_Occured");





            }









        }

        public async Task<GenericOperationResult<ProductMediaModel>> GetByIdAsync(int id)
        {
            try
            {
                if (await mediaRepo.IsProductMediaExist(id))
                {
                    return GenericOperationResult<ProductMediaModel>.ToFail("failed to get", new List<string> { "this peorduct media does not exist" }
                    , "ProductMedia_NotExist"); 


                }
                var media = await mediaRepo.GetByIdAsync(id);
                var model = new ProductMediaModel
                {
                    ConfirmByUserName = media.ConfirmByUserName,
                    DisplayOrder = media.DisplayOrder,
                    fileSize = media.fileSize,
                    IsConfirmed = media.IsConfirmed,
                    MediaTypeId = media.MediaTypeId,
                    ProductId = media.ProductId,
                    UploadingUserName = media.UploadingUserName,
                    Url = media.Url,
                    ProductMediaId = media.ProductMediaId,




                };
                return GenericOperationResult<ProductMediaModel>.ToSuccess("get succeed", model);
            }
            catch (Exception ex) {
                logger.LogError(ex,"faled get productmedia {ProductMediaId}",id);
                return GenericOperationResult<ProductMediaModel>.ToFail("failed to get", new List<string> {"an unexpected error occured"},"Exception_Occured");
            }



        }
        

        public async Task<GenericOperationResult<List<ProductMediaModel>>> GetByProductIdAsync(int productId)
        {
            try
            {
                var MediaList = await mediaRepo.GetByProductIdAsync(productId);
                var modelList = MediaList.Select(x => new ProductMediaModel
                {
                    ConfirmByUserName = x.ConfirmByUserName,
                    DisplayOrder = x.DisplayOrder,
                    fileSize = x.fileSize,
                    IsConfirmed = x.IsConfirmed,
                    MediaTypeId = x.MediaTypeId,
                    UploadingUserName = x.UploadingUserName,
                    ProductId = x.ProductId,
                    Url = x.Url,
                    ProductMediaId=x.ProductMediaId ,

                }).ToList();

                return GenericOperationResult<List<ProductMediaModel>>.ToSuccess("list items", modelList);



            }
            catch (Exception ex) {

                logger.LogError(ex, "failed to get product media list {productid} ", productId);

               return  GenericOperationResult<List<ProductMediaModel>>.ToFail("faile to get", new List<string> { "An unexpected error occures" },
                "Exception_Occured");


            }
        }

        public async Task<OperationResult> RemoveAsync(int id)
        {
            try {
                var item = await mediaRepo.GetByIdAsync(id);
                if (item==null)
                {
                    return OperationResult.ToFail("failed to remove", new List<string> { "this product media does  not exist" }, "ProductMedia_Not_Exist");

                }

                await fileStorage.DeleteAsync(item.Url);

                await mediaRepo.RemoveAsync(id);
                return OperationResult.ToSuccess(id, "the media removed successfully ");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to remove product media {id}", id);

                return OperationResult.ToFail(id,"failed to remove product media" ,
                    new List<string> {"an unexpected error occured"},"Exception_Occerd"  );
            
            }


        }
    }
}
