using EndWerk.API.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Object;
using Order.Services;

namespace EndWerk.API.Controllers
{
    public class ProductController : Controller
    {
        private ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        // GET: ProductController
        public ActionResult GetProducts()
        {
            try
            {
                var products = _productService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message ="couldnt find a list of products" });
            }
            
        }

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

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: ProductController/Create
        [HttpPost("create")]
        public ActionResult Create([FromBody]ProductDTO product)
        {
            try
            {
                var productToCreate = _productService.CreateOrUpdateProduct(
                    new Product
                    {
                        //ProductId = product.ProductId,
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

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
