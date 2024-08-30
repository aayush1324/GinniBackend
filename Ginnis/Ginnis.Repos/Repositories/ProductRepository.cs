using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _authContext;

        public ProductRepository(AppDbContext context)
        {
            _authContext = context;
        }



        public async Task<IActionResult> GetProductsWithImage(Guid userID)
        {
            var products = await _authContext.ProductLists
                                       .Select(product => new
                                       {
                                           product.Id,
                                           product.ProductName,
                                           product.Url,
                                           product.Price,
                                           product.Discount,
                                           product.DiscountCoupon,
                                           product.DiscountedPrice,
                                           product.DeliveryPrice,
                                           product.Quantity,
                                           product.Description,
                                           product.Category,
                                           product.Subcategory,
                                           product.Weight,
                                           product.Status,
                                           product.UnitSold,
                                           product.UnitLeft,
                                           product.Rating ,
                                           product.UserRating,                                           // Include other properties you want
                                           // Exclude ProfileImage and ImageData
                                       })
                                       .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return new NotFoundResult();
            }
                    
            var cartItems = await _authContext.Carts
                                            .Where(item => item.UserId == userID)
                                            .Select(item => item.ProductId)
                                            .ToListAsync();

            var wishlistItems = await _authContext.Wishlists
                                                .Where(item => item.UserId == userID)
                                                .Select(item => item.ProductId)
                                                .ToListAsync();


            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Url = product.Url,
                    Price = product.Price,
                    Discount = product.Discount,
                    Quantity = product.Quantity,
                    DiscountCoupon = product.DiscountCoupon,
                    DeliveryPrice = product.DeliveryPrice,
                    DiscountedPrice = product.DiscountedPrice,
                    Description = product.Description,
                    Category = product.Category,
                    Subcategory = product.Subcategory,
                    Weight = product.Weight,
                    Status = product.Status,
                    UnitSold = product.UnitSold,
                    UnitLeft = product.UnitLeft,
                    Rating = product.Rating,
                    UserRating = product.UserRating,
                    //ProfileImage = product.ProfileImage,
                    //ImageData = product.ImageData,
                    InCart = cartItems.Contains(product.Id), // Check if the product is in the cart
                    InWishlist = wishlistItems.Contains(product.Id) // Check if the product is in the wishlist
                };

                productDTOs.Add(productDTO);
            }

            return new OkObjectResult(productDTOs);
        }


        public async Task<IActionResult> GetProductsWithImages()
        {
            //var products = await _authContext.ProductLists.ToListAsync();

            var products = await _authContext.ProductLists
                                        .Select(product => new
                                        {
                                            product.Id,
                                            product.ProductName,
                                            product.Url,
                                            product.Price,
                                            product.Discount,
                                            product.Quantity,
                                            product.DiscountCoupon,
                                            product.DiscountedPrice,
                                            product.DeliveryPrice,
                                            product.Description,
                                            product.Category,
                                            product.Subcategory,
                                            product.Weight,
                                            product.Status,
                                            product.UnitSold,
                                            product.UnitLeft,
                                            product.Rating,
                                            product.UserRating,
                                            // Include other properties you want
                                            // Exclude ProfileImage and ImageData
                                        })
                                        .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return new NotFoundResult();
            }

            var productDTOs = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Url = product.Url,
                    Price = product.Price,
                    Discount = product.Discount,
                    Quantity  = product.Quantity,
                    DiscountCoupon = product.DiscountCoupon,
                    DiscountedPrice = product.DiscountedPrice,
                    DeliveryPrice = product.DeliveryPrice,
                    Description = product.Description,
                    Category = product.Category,
                    Subcategory = product.Subcategory,
                    Weight = product.Weight,
                    Status = product.Status,
                    UnitSold = product.UnitSold,
                    UnitLeft = product.UnitLeft,
                    Rating = product.Rating,
                    UserRating = product.UserRating,
                    //ProfileImage = product.ProfileImage,
                    //ImageData = product.ImageData,
                    InCart = false,
                    InWishlist = false
                };

                productDTOs.Add(productDTO);
            }

            return new OkObjectResult(productDTOs);
        }


        //public async Task<IActionResult> AddImageToProduct(ProductList product, IFormFile image)
        //{
        //    if (image == null || image.Length == 0)
        //        return new BadRequestObjectResult("Invalid file");

        //    if (product == null)
        //        return new BadRequestResult();

        //    product.Created_at = DateTime.Now;

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await image.CopyToAsync(memoryStream);
        //        product.ProfileImage = memoryStream.ToArray();
        //        product.ImageData = Convert.ToBase64String(memoryStream.ToArray());
        //    }

        //    var productAdded = _authContext.ProductLists.Add(product);
        //    await _authContext.SaveChangesAsync();

        //    return new OkObjectResult(new
        //    {
        //        ProductId = productAdded.Entity.Id.ToString(),
        //        Message = "Product Added Successfully!"
        //    });
        //}

        public async Task<IActionResult> AddImageToProduct(ProductList product)
        {
            //if (image == null || image.Length == 0)
            //    return new BadRequestObjectResult("Invalid file");

            if (product == null)
                return new BadRequestResult();

            product.Created_at = DateTime.Now;

            //using (var memoryStream = new MemoryStream())
            //{
            //    await image.CopyToAsync(memoryStream);
            //    product.ProfileImage = memoryStream.ToArray();
            //    product.ImageData = Convert.ToBase64String(memoryStream.ToArray());
            //}

            var productAdded = _authContext.ProductLists.Add(product);
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                ProductId = productAdded.Entity.Id.ToString(),
                Message = "Product Added Successfully!"
            });
        }

        public async Task<IActionResult> EditProduct(Guid productId, ProductList updatedProduct, IFormFile image)
        {
            try
            {
                var product = await _authContext.ProductLists.FindAsync(productId);

                if (product == null)
                {
                    return new NotFoundResult();
                }

                // Update product properties with the new values
                product.ProductName = updatedProduct.ProductName;
                product.Url = updatedProduct.Url;
                product.Price = updatedProduct.Price;
                product.Discount = updatedProduct.Discount;
                product.DeliveryPrice = updatedProduct.DeliveryPrice;
                product.DiscountCoupon = updatedProduct.DiscountCoupon;
                product.DiscountedPrice = updatedProduct.DiscountedPrice;
                product.Quantity = updatedProduct.Quantity;
                product.Description = updatedProduct.Description;
                product.Category = updatedProduct.Category;
                product.Subcategory = updatedProduct.Subcategory;
                product.Weight = updatedProduct.Weight;
                product.Status = updatedProduct.Status;
                product.UnitSold = updatedProduct.UnitSold;
                product.UnitLeft = updatedProduct.UnitLeft;
                product.Rating = updatedProduct.Rating;
                product.UserRating = updatedProduct.UserRating;
             
                product.Modified_at = DateTime.Now;


                if (image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        product.ProfileImage = memoryStream.ToArray();
                        product.ImageData = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "Product updated successfully!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                var product = await _authContext.ProductLists.FindAsync(productId);
                var images = await _authContext.Images.Where(img => img.ProductId == productId).ToListAsync();

                if (product == null)
                {
                    return new NotFoundResult();
                }

                _authContext.ProductLists.Remove(product);
                _authContext.Images.RemoveRange(images);

                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "DeleteProduct Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        public IActionResult Search(string term)
        {
            var results = _authContext.ProductLists
                                        .Where(p => p.ProductName.Contains(term))
                                        .Select(p => new
                                        {
                                            ProductName = p.ProductName,
                                            Price = p.Price,
                                            Discount = p.Discount,
                                            Quantity = p.Quantity,
                                            DiscountCoupon = p.DiscountCoupon,
                                            DiscountedPrice = p.DiscountedPrice,
                                            DeliveryPrice = p.DeliveryPrice,
                                            Description = p.Description,
                                            Category = p.Category,
                                            Subcategory = p.Subcategory,
                                            Weight = p.Weight,
                                            Status = p.Status,
                                            UnitSold = p.UnitSold,
                                            UnitLeft = p.UnitLeft,
                                            Rating = p.Rating,
                                            UserRating = p.UserRating,

                                            //ImageData = p.ImageData,
                                            //ProfileImage = p.ProfileImage,
                                        })
                                        .Take(5)
                                        .ToList();

            return new OkObjectResult(results);
        }


        public async Task<IActionResult> GetProduct()
        {
            //var productlist = await _authContext.ProductLists.ToListAsync();

            var productlist =  await _authContext.ProductLists
                                        .Select(product => new
                                        {
                                            product.Id,
                                            product.ProductName,
                                            product.Url,
                                            product.Price,
                                            product.Discount,
                                            product.DiscountCoupon,
                                            product.DiscountedPrice,
                                            product.DeliveryPrice,
                                            product.Description,
                                            product.Category,
                                            product.Subcategory,
                                            product.Weight,
                                            product.Status,
                                            product.UnitSold,
                                            product.UnitLeft,
                                            product.Rating,
                                            product.UserRating,
                                            // Include other properties you want
                                            // Exclude ProfileImage and ImageData
                                        })
                                        .ToListAsync();

            if (productlist == null || productlist.Count == 0)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(productlist);
        }


        public async Task<IActionResult> GetProductByName(string productName)
        {
            //var product = await _authContext.ProductLists.FirstOrDefaultAsync(p => p.ProductName == productName);

            var product = await _authContext.ProductLists
                                            .Where(p => p.ProductName == productName)
                                            .Select(p => new
                                            {
                                                p.Id,
                                                p.ProductName,
                                                p.Price,
                                                p.Discount,
                                                p.DiscountCoupon,
                                                p.DiscountedPrice,
                                                p.DeliveryPrice,
                                                p.Description,
                                                p.Category,
                                                p.Subcategory,
                                                p.Weight,
                                                p.Status,
                                                p.Rating,
                                                p.UserRating,
        
                                                // Add other properties you want to include
                                            })
                                            .FirstOrDefaultAsync();

            if (product == null)
            {
                return new NotFoundResult();
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Discount = product.Discount,
                DiscountCoupon = product.DiscountCoupon,
                DiscountedPrice = product.DiscountedPrice,
                DeliveryPrice = product.DeliveryPrice,
                Weight = product.Weight,
                Status = product.Status,
                Description = product.Description,
                Category = product.Category,
                Subcategory = product.Subcategory,
                Rating = product.Rating,
                UserRating = product.UserRating,

                //ProfileImage = product.ProfileImage,
                //ImageData = product.ImageData
            };

            return new OkObjectResult(productDTO);
        }



        public async Task<IActionResult> GetProductImage(Guid productId)
        {
            var product = await _authContext.ProductLists.FindAsync(productId);

            if (product == null || product.ProfileImage == null)
            {
                return new NotFoundResult();
            }

            return new FileContentResult(product.ProfileImage, "image/jpeg");
        }

    }
}
