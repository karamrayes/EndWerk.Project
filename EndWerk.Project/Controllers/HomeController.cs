using EndWerk.Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Order.Object;
using Order.Services;
using System.Diagnostics;

namespace EndWerk.Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MessageServices _messageServices { get; set; }

        public HomeController(ILogger<HomeController> logger , MessageServices messageServices)
        {
            _logger = logger;
            _messageServices = messageServices;
        }

        public IActionResult Index()
        {
            
                // Retrieve the list of messages from your data source
                List<Order.Object.Message> messages = _messageServices.GetMessages();
                         
            return View(messages);
        }

        public IActionResult Privacy()
        {           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}