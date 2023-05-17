using EndWerk.Project.Data;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Order.Object;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class ProductController : Controller
    {
        
        private ProductService _productService;
        private SupplierService _SupplierService;
        private ProductCategoryService _ProductCategoryService;

        public ProductController(ProductService productService , SupplierService supplierService , ProductCategoryService ProductCategory) 
        {
            this._productService = productService;
            _SupplierService = supplierService;
            _ProductCategoryService = ProductCategory;
        }
        public IActionResult Index()
        {
            var list = _productService.GetProducts();

            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }


            return View(list);
        }

        public IActionResult Details(int id) 
        {
            var product = _productService.GetProduct(id);
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(product);
        }

        [Authorize]
        public IActionResult Edit(int id) 
        {
            ViewData["supplierId"] = new SelectList(_SupplierService.GetSuppliers(), "SupplierId", "SupplierId");
            ViewData["ProductCategoryId"] = new SelectList(_ProductCategoryService.GetProductCatagoryList(), "ProductCategoryId", "ProductCategoryId");

            var product = _productService.GetProduct(id);
            
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id ,Product product)
        {

            ViewData["supplierId"] = new SelectList(_SupplierService.GetSuppliers(), "SupplierId", "Id", product.supplierId);
            ViewData["ProductCategoryId"] = new SelectList(_ProductCategoryService.GetProductCatagoryList(), "ProductCategoryId", "ProductCategoryId", product.ProductCategory);

            var ProductToEdit = _productService.GetProduct(id);
            TryUpdateModelAsync(ProductToEdit);
            var Result = _productService.CreateOrUpdateProduct(ProductToEdit);
            if (Result != null)
            {
                TempData["Message"] = "Object has been updated successfully";
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                TempData["Message"] = "Error Couldnt Update";
                return RedirectToAction("Edit", new { id = id });
            }

            
        }

        [Authorize]
        public IActionResult Create() 
        {
            //var x = _SupplierService.GetSuppliers();
            ViewData["supplierId"] = new SelectList(_SupplierService.GetSuppliers(), "SupplierId", "SupplierId");
            ViewData["ProductCategoryId"] = new SelectList(_ProductCategoryService.GetProductCatagoryList(), "ProductCategoryId", "ProductCategoryId");
            
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Product product)
        {
            
            ViewData["supplierId"] = new SelectList(_SupplierService.GetSuppliers(), "SupplierId", "Id" ,product.supplierId);
            ViewData["ProductCategoryId"] = new SelectList(_ProductCategoryService.GetProductCatagoryList(), "ProductCategoryId", "ProductCategoryId",product.ProductCategory);

            var Result =_productService.CreateOrUpdateProduct(product);
            if (Result != null)
            {
                TempData["Message"] = "Object has been Created successfully";
                return RedirectToAction("Details", new {id = Result.ProductId});
            }
            else 
            {
                TempData["Message"] = "Error Couldnt Create Product";
                return RedirectToAction("Create");
            }           
        }

        [Authorize]
        public IActionResult Delete(int id) 
        {
           var product=  _productService.GetProduct(id);

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id , Product product)
        {
            var Result = _productService.DeleteProduct(id);
            if (Result)
            {
                TempData["Message"] = "Object has been Deleted successfully";
                return RedirectToAction("Index");
                
            }
            else
            {
                TempData["Message"] = "Error Couldnt Delete";
                return RedirectToAction("Delete", new { id = id });
               
            }
                
        }
    }
}
