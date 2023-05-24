using Microsoft.AspNetCore.Mvc;
using Order.Object;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class MessageController : Controller
    {
        private MessageServices _messageServices { get; set; }
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        public MessageController(MessageServices messageServices, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _messageServices = messageServices;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            
            var messages = _messageServices.GetMessages();
            

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(messages);
        }

        public IActionResult Details(int id)
        {
            var message = _messageServices.GetMessage(id);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(message);
        }

        public IActionResult Edit(int id)
        {
            var message = _messageServices.GetMessage(id);

            return View(message);
        }

        [HttpPost]
        public IActionResult Edit(int id, Message message)
        {
            var MessageToEdit = _messageServices.GetMessage(id);

            TryUpdateModelAsync(MessageToEdit);
            _messageServices.UpdataOrCreateMessage(MessageToEdit);
            TempData["message"] = "Object has been updated successfully.";

            return RedirectToAction("Details", new { id = MessageToEdit.BerichtId });
        }

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
            return View();
        }

        [HttpPost]
        public IActionResult Create(Message message)
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

            _messageServices.UpdataOrCreateMessage(message);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Details", new { id = message.BerichtId });
        }


        public IActionResult Delete(int id)
        {
            var MessageToDelete = _messageServices.GetMessage(id);

            return View(MessageToDelete);
        }

        [HttpPost]
        public IActionResult Delete(int id, User user)
        {
            var MessageToDelete = _messageServices.GetMessage(id);

            _messageServices.DeleteMessage(id);

            TempData["message"] = "Object has been Deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}

