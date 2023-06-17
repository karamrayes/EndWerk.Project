//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Order.Object;
using Order.Project.Web.Models;
using Order.Services;

namespace Order.Project.Web.Controllers
{

    public class OrderController : Controller
    {

        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private OrderService _orderService { get; set; }

        private OrderDetailsService _OrderDetailsService { get; set; }
        private ProductService _productService { get; set; }

        public OrderController(OrderService orderService, ProductService productService, OrderDetailsService orderDetailsService, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
            _productService = productService;
            _OrderDetailsService = orderDetailsService;
        }

        [Authorize]
        public IActionResult Index()
        {

            var list = _orderService.GetOrders();

            
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(list);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var Order = _orderService.GetOrder(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View(Order);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var order = _orderService.GetOrder(id);

            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(order);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, Order.Object.Order order)
        {


            var orderToEdit = _orderService.GetOrder(id);
            TryUpdateModelAsync(orderToEdit);
            var Result = _orderService.UpdateOrCreateOrder(orderToEdit);

            if (Result != null)
            {
                TempData["message"] = "Object has been updated successfully.";
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                TempData["message"] = "Error Couldnt Update";
                return RedirectToAction("Edit", new { id = id });
            }

        }

        [Authorize]
        public IActionResult Create()
        {
            
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != null)
            {
                ViewData["UserId"] = currentUserId;
            }
            else
            {
                ViewData["UserId"] = "";
            }

            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View();
        }
        

        [Authorize]
        public IActionResult Delete(int id)
        {
            var ordertodelete = _orderService.GetOrder(id);

            return View(ordertodelete);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id, Order.Object.Order order)
        {
            var ordertodelete = _orderService.GetOrder(id);

            _productService.UpdateProductUnitInstock(ordertodelete.OrderDetail ,true);

            var Result = _orderService.DeleteOrder(id);
            
            

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

        [Authorize]
        public IActionResult CreateOrder()
        {
            var currentUserId = _userManager.GetUserId(User);

            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            OrderDetailsModel model = new OrderDetailsModel();
            model.OrderDetailsList = new List<OrderDetail>();


            var products = _productService.GetProducts()
                          .Where(p => p.UnitInStock > 0)
                          .Select(p => new
             {
                           Id = p.ProductId,
                           price = p.ProductPrice,
                           DisplayText = $"ID: {p.ProductId} - Price: {p.ProductPrice} - Name: {p.ProductName} - UnitInStock: {p.UnitInStock}"
             });;

            ViewBag.ProductIdList = new SelectList(products, "Id", "DisplayText");

            ViewBag.ProductPrices = new SelectList(products, "Id", "price");

            ViewBag.UserId = currentUserId;

            ViewBag.UserName = _userManager.GetUserName(User);


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(OrderDetailsModel model)
        {
           
           
            // Validate and process the order         
            foreach (var item in model.OrderDetailsList)
            {
                //get unit price 
                item.UnitPrice = _productService.GetProduct(item.ProductId).ProductPrice;
                
            }

            // Calculate the total amount
            var totalAmount = _orderService.CalculateOrderAmount(model.OrderDetailsList);


            // Assign the total amount to the Order.Amount property
            model.Order.OrderAmount = totalAmount;
            
                        
            //CheckUnitInStock , no Stock is false
            if (_orderService.CheckUnitInStock(model.OrderDetailsList) == false)
            {
                TempData["message"] = "Chosen Quantity is above UnitInStock";


                return RedirectToAction("CreateOrder");

            }
            
            _orderService.MakeOrder(model.Order, model.OrderDetailsList);

            return RedirectToAction("Details", new { id = model.Order.OrderId });
        }
    }
}
