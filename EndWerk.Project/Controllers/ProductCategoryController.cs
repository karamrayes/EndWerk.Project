using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Object;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ProductCategoryService _productCategoryService { get; set; }

        public ProductCategoryController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var suppliers = _productCategoryService.GetProductCatagoryList();


            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(suppliers);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var user = _productCategoryService.GetProductCatagory(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(user);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var user = _productCategoryService.GetProductCatagory(id);

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, ProductCategory user)
        {
            var userToEdit = _productCategoryService.GetProductCatagory(id);

            TryUpdateModelAsync(userToEdit);
            _productCategoryService.UpdateOrCreateProductCategory(userToEdit);
            TempData["message"] = "Object has been updated successfully.";

            return RedirectToAction("Details", new { id = userToEdit.ProductCategoryId });
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProductCategory user)
        {
            var UserToCreate = _productCategoryService.UpdateOrCreateProductCategory(user);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Details", new { id = UserToCreate.ProductCategoryId });
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            var UserToDelte = _productCategoryService.GetProductCatagory(id);

            return View(UserToDelte);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id, Supplier user)
        {
            var UserToDelte = _productCategoryService.GetProductCatagory(id);

            _productCategoryService.DeleteProductCategory(id);

            TempData["message"] = "Object has been Deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}

