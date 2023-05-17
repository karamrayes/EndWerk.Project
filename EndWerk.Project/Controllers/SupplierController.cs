using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Object;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class SupplierController : Controller
    {
        private SupplierService _supplierService { get; set; }

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var suppliers = _supplierService.GetSuppliers();
            

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(suppliers);
        }


        [Authorize]
        public IActionResult Details(int id)
        {
            var user = _supplierService.GetSupplier(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(user);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            var user = _supplierService.GetSupplier(id);

            return View(user);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, Supplier user)
        {
            var userToEdit = _supplierService.GetSupplier(id);

            TryUpdateModelAsync(userToEdit);
            _supplierService.UpdateOrCreateSupplier(userToEdit);
            TempData["message"] = "Object has been updated successfully.";

            return RedirectToAction("Details", new { id = userToEdit.SupplierId});
        }


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Supplier user)
        {
            var UserToCreate = _supplierService.UpdateOrCreateSupplier(user);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Details", new { id = UserToCreate.SupplierId });
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            var UserToDelte = _supplierService.GetSupplier(id);

            return View(UserToDelte);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id, Supplier user)
        {
            var UserToDelte = _supplierService.GetSupplier(id);

            _supplierService.DeleteSupplier(id);

            TempData["message"] = "Object has been Deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}

