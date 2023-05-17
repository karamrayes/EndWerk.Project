using Microsoft.AspNetCore.Mvc;
using Order.Object;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class OrderDetailsController : Controller
    {
        private OrderDetailsService _OrderDetailsService { get; set; }

        public OrderDetailsController(OrderDetailsService orderDetailsService)
        {
            _OrderDetailsService = orderDetailsService;
        }

        public IActionResult Index() 
        {
            var x = _OrderDetailsService.GetOrderDetails();
             
           return View(x);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(OrderDetail user)
        {
            var UserToCreate = _OrderDetailsService.UpdateOrCreateOrderDetails(user);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Index");
        }
    }
}
