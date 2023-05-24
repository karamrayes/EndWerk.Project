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

            //var currentUserId = User.Identity.GetUserId();
            //var currentUser = _userManager.GetUserAsync;

            //var listoforderdetails = _OrderDetailsService.GetOrderDetails();

            var list = _orderService.GetOrders();

            //var UserModel = list.Select(Order => new OrderModel
            //{
            //    OrderId = Order.OrderId,
            //    OrderAmount = Order.OrderAmount,
            //    OrderDate = Order.OrderDate,
            //    ShipDate = Order.ShipDate,
            //    Shipped = Order.Shipped,
            //    PaymentRecevied = Order.PaymentRecevied,
            //    UserId = Order.UserId,
            //    User = Order.User,
            //    OrderDetails = Order.OrderDetails
            //    //Idmodel == user.Id
            //}).ToList();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(list);
        }

        [Authorize]
        public IActionResult Index2()
        {

            //var currentUserId = User.Identity.GetUserId();
            //var currentUser = _userManager.GetUserAsync;

            //var listoforderdetails = _OrderDetailsService.GetOrderDetails();

            var list = _orderService.GetOrders();

            //var UserModel = list.Select(Order => new OrderModel
            //{
            //    OrderId = Order.OrderId,
            //    OrderAmount = Order.OrderAmount,
            //    OrderDate = Order.OrderDate,
            //    ShipDate = Order.ShipDate,
            //    Shipped = Order.Shipped,
            //    PaymentRecevied = Order.PaymentRecevied,
            //    UserId = Order.UserId,
            //    User = Order.User,
            //    OrderDetails = Order.OrderDetails
            //    //Idmodel == user.Id
            //}).ToList();
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
            //var currentUserId = User.Identity.GetUserId();
            //var currentUser = _userManager.GetUserAsync;
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
        [HttpPost]
        public IActionResult Create(Order.Object.Order order)
        {
            //var currentUserId = User.Identity.GetUserId();
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != null)
            {
                ViewData["UserId"] = currentUserId;
            }
            else
            {
                ViewData["UserId"] = "";
            }

            var Result = _orderService.UpdateOrCreateOrder(order);
            if (Result != null)
            {
                TempData["message"] = "Object has been Created successfully.";
                return RedirectToAction("Details", new { id = Result.OrderId });
            }
            else
            {
                TempData["message"] = "Error Couldnt Create";
                return RedirectToAction("Create");
            }

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
            OrderDetailsModel model = new OrderDetailsModel();
            model.OrderDetailsList = new List<OrderDetail>();


            var products = _productService.GetProducts().Select(p => new
            {
                Id = p.ProductId,
                DisplayText = $"ID: {p.ProductId} - Price: {p.ProductPrice} -Name: {p.ProductName}"
            });

            ViewBag.ProductIdList = new SelectList(products, "Id", "DisplayText");

            //ViewData["ProductId"] = new SelectList(_productService.GetProducts(), "ProductId", "ProductId");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(OrderDetailsModel model, int id, string submitButton)
        {
            var currentUserId = _userManager.GetUserId(User);

            var products = _productService.GetProducts().Select(p => new
            {
                Id = p.ProductId,
                DisplayText = $"ID: {p.ProductId} - Price: {p.ProductPrice} -Name: {p.ProductName}"
            });

            ViewBag.ProductIdList = new SelectList(products, "Id", "DisplayText");

            // Validate and process the order

            // Calculate the total amount
            decimal totalAmount = 0;
            foreach (var item in model.OrderDetailsList)
            {
                item.UnitPrice = _productService.GetProduct(item.ProductId).ProductPrice;

                totalAmount += item.Quantity * item.UnitPrice;
            }
            

            // Assign the total amount to the Order.Amount property
            model.Order.OrderAmount = totalAmount;
            model.Order.OrderDate = DateTime.Now;
            model.Order.UserId = currentUserId;
            model.Order.Shipped = false;

            _orderService.MakeOrder(model.Order, model.OrderDetailsList);

            return RedirectToAction("Details", new { id = model.Order.OrderId });
        }
    }
}
