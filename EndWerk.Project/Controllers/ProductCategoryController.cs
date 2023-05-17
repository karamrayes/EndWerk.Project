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
        public IActionResult Index()
        {
            var suppliers = _productCategoryService.GetProductCatagoryList();


            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(suppliers);
        }

        public IActionResult Details(int id)
        {
            var user = _productCategoryService.GetProductCatagory(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var user = _productCategoryService.GetProductCatagory(id);

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductCategory user)
        {
            var userToEdit = _productCategoryService.GetProductCatagory(id);

            TryUpdateModelAsync(userToEdit);
            _productCategoryService.UpdateOrCreateProductCategory(userToEdit);
            TempData["message"] = "Object has been updated successfully.";

            return RedirectToAction("Details", new { id = userToEdit.ProductCategoryId });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductCategory user)
        {
            var UserToCreate = _productCategoryService.UpdateOrCreateProductCategory(user);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Details", new { id = UserToCreate.ProductCategoryId });
        }


        public IActionResult Delete(int id)
        {
            var UserToDelte = _productCategoryService.GetProductCatagory(id);

            return View(UserToDelte);
        }

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

