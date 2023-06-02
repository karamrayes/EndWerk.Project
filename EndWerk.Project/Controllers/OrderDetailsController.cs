using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Order.Object;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class OrderDetailsController : Controller
    {
        private OrderDetailsService _OrderDetailsService { get; set; }

        private ProductService _productService { get; set; }

        private OrderService _orderService { get; set; }

        public OrderDetailsController(OrderDetailsService orderDetailsService, ProductService prouctService, OrderService orderService)
        {
            _OrderDetailsService = orderDetailsService;
            _productService = prouctService;
            _orderService = orderService;
        }

        [Authorize]
        public IActionResult Index() 
        {
            var x = _OrderDetailsService.GetOrderDetails();
             
           return View(x);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(OrderDetail user)
        {
            var UserToCreate = _OrderDetailsService.UpdateOrCreateOrderDetails(user);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var products = _productService.GetProducts()
                          .Where(p => p.UnitInStock > 0)
                          .Select(p => new
                          {
                              Id = p.ProductId,
                              DisplayText = $"ID: {p.ProductId} - Price: {p.ProductPrice} - Name: {p.ProductName}"
                          });

            ViewBag.ProductIdList = new SelectList(products, "Id", "DisplayText");

            var result = _OrderDetailsService.GetOrderDetail(id);

            return View(result);
        }

        [Authorize]
        [HttpPost]        
        public IActionResult Edit(int id, OrderDetail orderDetail)
        {
            var products = _productService.GetProducts()
                          .Where(p => p.UnitInStock > 0)
                          .Select(p => new
                          {
                              Id = p.ProductId,
                              DisplayText = $"ID: {p.ProductId} - Price: {p.ProductPrice} - Name: {p.ProductName}"
                          });

            ViewBag.ProductIdList = new SelectList(products, "Id", "DisplayText");

            var orderdetailtoupdate = _OrderDetailsService.GetOrderDetail(id);

            //update UntiPrice
            orderdetailtoupdate.UnitPrice = _productService.GetProduct(orderdetailtoupdate.ProductId).ProductPrice;

            TryUpdateModelAsync(orderdetailtoupdate);

            _OrderDetailsService.UpdateOrCreateOrderDetails(orderdetailtoupdate);

            var FilteredList = _OrderDetailsService.GetOrderDetails().Where(od => od.OrderId == orderdetailtoupdate.OrderId).ToList();

            
           
            //updateOrder Amount
            var ordertoupdate = _orderService.GetOrder(orderdetailtoupdate.OrderId);

            ordertoupdate.OrderAmount = _orderService.CalculateOrderAmount(FilteredList);
            
            TryUpdateModelAsync(ordertoupdate);

            _orderService.UpdateOrCreateOrder(ordertoupdate);

            TempData["message"] = "Object has been updated successfully.";

            return RedirectToAction("Index");
            //return RedirectToAction("Details", new { id = orderdetailtoupdate.OrderDetailId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllOrderDetailsById(int id) 
        {
            var list = _OrderDetailsService.GetOrderDetails().Where(od => od.OrderId == id).ToList();

            return View(list);
        }
    }
}
