using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EndWerk.Project.Data;
using Order.Object;
using Order.Services;
using EndWerk.API2.Dto;

namespace EndWerk.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                var products =_productService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "couldnt find a list of products" });
            }

        }

        // GET: api/Product/5        
        [HttpGet("{id}")]
        public ActionResult GetProductById(int id)
        {
            try
            {
                var product = _productService.GetProduct(id);
                return Ok(product);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "couldnt find the product" });
            }
        }

        [HttpPut("update/{id}")] // volledige resource updaten
        public IActionResult Update(int id, [FromBody] ProductDTO product)
        {

            try
            {
                //car met id opzoeken
                var productToUpdate = _productService.CreateOrUpdateProduct( new Product
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    UnitInStock = product.UnitInStock,
                    ProductCategoryId = product.ProductCategoryId,
                    supplierId = product.supplierId

                });

                if (productToUpdate == null)// 404 wanneer null
                {
                    return NotFound("Car not found");
                }

                return CreatedAtAction("GetProductById", new { id = productToUpdate.ProductId } ,productToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public ActionResult Create([FromBody] ProductDTO product)
        {
            try
            {
                var productToCreate = _productService.CreateOrUpdateProduct(
                    new Product
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductDescription = product.ProductDescription,
                        ProductPrice = product.ProductPrice,
                        UnitInStock = product.UnitInStock,
                        ProductCategoryId = product.ProductCategoryId,
                        supplierId = product.supplierId
                    });

                return Ok(productToCreate);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Couldnt Create Product" });
            }

        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                //filter het car objectje met deze id eruit
                var isDeleted =_productService.DeleteProduct(id);

                if (isDeleted)
                {

                    //retourneer een nieuw OK response 
                    return Ok(new { Message = "product Deleted successfully" });
                }

                return BadRequest(new { Message = "Something went wrong trying to delete car." });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, (new { Message = "Something went wrong please try again" }));
            }
        }

        //private bool ProductExists(int id)
        //{
        //    return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        //}
    }
}