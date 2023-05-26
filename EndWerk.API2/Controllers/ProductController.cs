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

namespace EndWerk.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Repository _context;

        public ProductController(Repository context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'Repository.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}


//public ActionResult GetProducts()
//{
//    try
//    {
//        var products = _productService.GetProducts();
//        return Ok(products);
//    }
//    catch (Exception ex)
//    {

//        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "couldnt find a list of products" });
//    }

//}

//[HttpGet("{id}")]
//public ActionResult GetProductById(int id)
//{
//    try
//    {
//        var product = _productService.GetProduct(id);
//        return Ok(product);
//    }
//    catch (Exception ex)
//    {

//        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "couldnt find the product" });
//    }
//}

//[HttpPost("create")]
//public ActionResult Create([FromBody] ProductDTO product)
//{
//    try
//    {
//        var productToCreate = _productService.CreateOrUpdateProduct(
//            new Product
//            {
//                //ProductId = product.ProductId,
//                ProductName = product.ProductName,
//                ProductDescription = product.ProductDescription,
//                ProductPrice = product.ProductPrice,
//                UnitInStock = product.UnitInStock,
//                ProductCategoryId = product.ProductCategoryId,
//                supplierId = product.supplierId
//            });

//        return Ok(productToCreate);
//    }
//    catch (Exception ex)
//    {

//        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Couldnt Create Product" });
//    }

//}