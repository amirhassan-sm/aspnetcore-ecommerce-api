using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.Contrast.Services;
using Application.DTOs.Product;
using Application.DTOs.ProductFeatures;
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
    public class ProductApplication : IProductApplicationServiceContract
    {
        private readonly IProductRepository repo;
        private readonly IProductQueryServices queryServices;
        private readonly ILogger<ProductApplication> logger;
        public ProductApplication(IProductRepository repo, IProductQueryServices queryServices)
        {
            this.repo = repo;
            this.queryServices = queryServices;

        }
        private Product AddModelMapToProduct(ProductAddModel model)
        {
            return new Product
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                CategoryId = model.CategoryId,
                BasePrice = model.BasePrice,
                ImageUrl = model.ImageUrl,
                Introduction = model.Introduction,
                AvailableProductCount = model.AvailableProductCount,

            };

        }
        private Product SeoModelMapToProduct(ProductSeoModel model)
        {
            return   new Product
            {
                ProductId = model.ProductId,
                ExtraData = model.ExtraData,
                BodyExtraData = model.BodyExtraData,
                Slug = model.Slug,
                PageTitle = model.PageTitle,
                MetaTag = model.MetaTag,
                MetaDescription = model.MetaDescription,


            };



        }
        private Product UpdateModelMapToProduct(ProductUpdateModel model)
        {
            return   new Product
            {
                ProductId = model.ProductId,
                ExtraData = model.ExtraData,
                BodyExtraData = model.BodyExtraData,
                Slug = model.Slug,
                PageTitle = model.PageTitle,
                MetaTag = model.MetaTag,
                MetaDescription = model.MetaDescription,
                AvailableProductCount= model.AvailableProductCount,
                BasePrice= model.BasePrice,
                CategoryId= model.CategoryId,
                ImageUrl= model.ImageUrl,
                Introduction= model.Introduction,
                ProductDescription= model.ProductDescription,
                ProductName= model.ProductName,
                
                


            };



        }

        public async Task<OperationResult> AddNew(ProductAddModel product)
        {
            try
            {
                if (await repo.ExistProductNameAsync(product.ProductName))
                {
                    return OperationResult.ToFail("add failed", new List<string> { "A product with this name already exists." });

                }


                if (!await repo.IsCategoryExist(product.CategoryId))
                {
                    return OperationResult.ToFail("add failed", new List<string> { "this category  does not exist" });

                }
                if (!await repo.IsCategoryLevel3(product.CategoryId))
                {
                    return OperationResult
                        .ToFail("add failed "
                        , new List<string> { "Only level 3 categories can contain products." });

                }



                int result = await repo.AddNew(AddModelMapToProduct(product));
                // result is prod id
                //assign the category feaures
                var catFeatures = await repo.GetProductCategoryFeatures(result);

                await repo.AsignAllCategoryFeaturesToProduct(result, catFeatures);

                return OperationResult.ToSuccess(result, "product added successfully");





            }
            catch (Exception ex)
            {
                logger.LogError(ex,
     "Failed to add product '{ProductName}' in category {CategoryId}.",
     product.ProductName,
     product.CategoryId);
                return OperationResult.ToFail("failed to add ", new List<string> { "an unexcpected error occures" });


            }

        }

        public async Task<GenericOperationResult<ProductModel>> Get(int ProductId)
        {
            try
            {
                var response = await repo.Get(ProductId);
                if (response == null)
                {
                    return GenericOperationResult<ProductModel>.ToFail(
                        "Product not found",
                        new List<string> { "The requested product does not exist." });
                }

                var result = new ProductModel
                {
                    ProductId = response.ProductId,
                    CategoryId = response.CategoryId,
                    AvailableProductCount = response.AvailableProductCount,
                    BasePrice = response.BasePrice,
                    BodyExtraData = response.BodyExtraData,
                    ExtraData = response.ExtraData,
                    ImageUrl = response.ImageUrl,
                    Introduction = response.Introduction,
                    MetaDescription = response.MetaDescription,
                    MetaTag = response.MetaTag,
                    PageTitle = response.PageTitle,
                    ProductDescription = response.ProductDescription,
                    ProductName = response.ProductName,
                    Slug = response.Slug,

                };

                return GenericOperationResult<ProductModel>.ToSuccess("get succeed", result);

            }
            catch (Exception ex)
            {
                logger.LogError(ex,
    "Failed to retrieve product {ProductId}.",
    ProductId);


                return GenericOperationResult<ProductModel>.ToFail("get failed", new List<string> { "An unexpected error occurred." });
            }



        }

        public async Task<GenericOperationResult<List<ProductFeaturesModel>>> GetAllProductFeatures(int ProductId)
        {
            try
            {
                if (!await repo.IsPrductExist(ProductId))
                {
                    return GenericOperationResult<List<ProductFeaturesModel>>
                        .ToFail("failed to get", new List<string> { "this product does not exist" });


                }
                var features = await repo.GetAllProductFeatures(ProductId);

                var featureModel = queryServices.GetProductFeatureModels(features);
                return GenericOperationResult<List<ProductFeaturesModel>>
                    .ToSuccess(ProductId, "product fetures list", featureModel);




            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to get product features", ProductId);
                return GenericOperationResult<List<ProductFeaturesModel>>
                    .ToFail(ProductId, "failed to get product features ", new List<string> { "an unexpected error occured" });





            }
        }

        public async Task<OperationResult> Remove(int ProductId)
        {
            try
            {
                if (!await repo.IsPrductExist(ProductId))
                {
                    return OperationResult.ToFail(ProductId, "remove failed", new List<string> { "this product does not exist" });

                }
                var result = await repo.Remove(ProductId);
                if (!result)
                {

                    return OperationResult.ToFail(ProductId, "remove failed", new List<string> { "an unexpected error occured" });

                }
                return OperationResult.ToSuccess(ProductId, "product removed succesfully");


            }
            catch (Exception ex)
            {
                logger.LogError(ex, " remove product failed ", ProductId);
                return OperationResult.ToFail(ProductId, "remove failed", new List<string> { "an unexpected error occured" });
            }
        }

        public async Task<GenericComplexResult<ProductSearchModel, ProductListIteam>> SearchProductAsync(ProductSearchModel model)
        {
            try
            {
                var result = await queryServices.SearchProductAsync(model);
                return result;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, " remove product failed ", model);

                 return new GenericComplexResult<ProductSearchModel, ProductListIteam> { } ;
            }
        }

        public async Task<OperationResult> setSeoAsync(ProductSeoModel Product)
        {
            try
            {

                if (!await repo.IsPrductExist(Product.ProductId))
                {
                    return OperationResult.ToFail("failed to set seo model", new List<string> { "this product does not exist" });
                }
                if (await repo.ProductExistExceptCurrentSlugAsync(Product.ProductId, Product.Slug))
                {
                    return OperationResult.ToFail(" failed to set seo model", new List<string> { "this slug already assigned to another product" });

                }

               await repo.setSeoAsync(SeoModelMapToProduct(Product));
               
                return OperationResult.ToSuccess(Product.ProductId , " seo model set to this prodcut successfully" );
            }
            catch (Exception ex) {
                logger.LogError(ex, "failed to set seo model {ProducyId}", Product.ProductId);
                return OperationResult.ToFail(" failed to set seo model", new List<string> { "an unexpected error occured" });

            }
        }

        public async Task<OperationResult> Update(ProductUpdateModel product)
        {
            try
            {

                if (!await repo.IsPrductExist(product.ProductId))
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this product does not exist" });
                }
               
                if (!await repo.IsCategoryLevel3(product.CategoryId))
                {
                    return OperationResult.ToFail("failed to update product", new List<string> { "this category is not level 3 or exist" });


                }
                if (await repo.ProductExistExceptCurrentNameAsync(product.ProductId , product.ProductName))
                {
                    return OperationResult.ToFail("failed to update product", new List<string> { "this product name assigned to another product" });

                }
                if (await repo.ProductExistExceptCurrentSlugAsync(product.ProductId, product.Slug))
                {
                    return OperationResult.ToFail("failed to update product", new List<string> { "this product slug assigned to another product" });

                }
                var result = await repo.Update(UpdateModelMapToProduct(product));
                if (!result)
                {
                    return OperationResult.ToFail("failed to update product", new List<string> { "an unexpected error occured" });

                }
                return OperationResult.ToSuccess(product.ProductId, "product updated");


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to update product {productId}", product.ProductId);
                return OperationResult.ToFail("failed to update product", new List<string> { "an unexpected error occured" });


            }
        }

        public async Task<OperationResult> AssignFeatureToProductAsync(int FeatureId, int ProductId)
        {
            try
            {
                if (!await repo.IsFeaureExistAsync(FeatureId))
                {
                    return OperationResult.ToFail("assgin failed", new List<string> { "this feature does not exist" });

                }
                if (!await repo.IsPrductExist(ProductId))
                {
                    return OperationResult.ToFail("assgin failed", new List<string> { "this product does not exist" });

                }
                if (await repo.IsProductFeatureExistAsync(ProductId, FeatureId))
                {
                    return OperationResult.ToFail("assgin failed", new List<string> { "this feature already assinged to the current product " });


                }
                await repo.AssignFeatureToProductAsync(FeatureId, ProductId);
                return OperationResult.ToSuccess(ProductId, $"the feature :{FeatureId} assgined to the this product");
            }
            catch (Exception ex) {

                logger.LogError(ex, "failed to assgin{FeatureId} {ProductId}", FeatureId,ProductId);
                return OperationResult.ToFail("assgin failed", new List<string> { "an unexpected error occured" });
            }


        }

        public async Task<OperationResult> RemoveFeaturesFromProductAsync(int FeatureId, int ProductId)
        {
            try
            {
                if (!await repo.IsFeaureExistAsync(FeatureId))
                {
                    return OperationResult.ToFail("failed to remove Feature", new List<string> { "this feature does not exist" });

                }
                if (!await repo.IsPrductExist(ProductId))
                {
                    return OperationResult.ToFail("failed to remove Feature", new List<string> { "this product does not exist" });

                }
                if (!await repo.IsProductFeatureExistAsync(ProductId, FeatureId))
                {
                    return OperationResult.ToFail("failed to remove Feature", new List<string> { "this feature does not assinged to the current product " });


                }
                await repo.RemoveFeaturesFromProductAsync(FeatureId, ProductId);
                return OperationResult.ToSuccess(ProductId, $"the feature :{FeatureId} removed from this product");
            }
            catch (Exception ex) {

                logger.LogError(ex, "failed to remove{FeatureId} {ProductId}", FeatureId, ProductId);
                return OperationResult.ToFail("failed to remove Feature", new List<string> { "an unexpected error occured" });


            }

        }
    }
}
