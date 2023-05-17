using Microsoft.AspNetCore.Mvc;
using Order.Object;
using Order.Project.Web.Models;
using Order.Services;

namespace Order.Project.Web.Controllers
{
    public class UserController : Controller
    { 
        private UserService _userService { get; set; }

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            var user = _userService.GetUsers();
            var UserModel = user.Select(user => new UserModel
            {
                Address = user.Address,
                Postcode = user.Postcode,
                City = user.City,
                Country = user.Country,
                Name = user.Name,
                DateBirth = user.DateBirth,
                //Idmodel == user.Id
            }).ToList();
           
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(UserModel);
        }

        public IActionResult Details(string id) 
        {
            var user = _userService.GetUser(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(user);
        }

        public IActionResult Edit(string id) 
        {
            var user = _userService.GetUser(id);

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(string id , User user)
        {
            var userToEdit = _userService.GetUser(id);

            _userService.UpdataOrCreateUser(userToEdit);
            TempData["message"] = "Object has been updated successfully.";

            return RedirectToAction("Details" ,new {id = userToEdit.Id});
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            var UserToCreate = _userService.UpdataOrCreateUser(user);
            TempData["message"] = "Object has been Created successfully.";

            return RedirectToAction("Details", new { id = UserToCreate.Id });
        }


        public IActionResult Delete(string id) 
        {
            var UserToDelte = _userService.GetUser(id);

            return View(UserToDelte);
        }

        [HttpPost]
        public IActionResult Delete(string id , User user)
        {
            var UserToDelte = _userService.GetUser(id);

            _userService.DeleteUser(id);

            TempData["message"] = "Object has been Deleted successfully.";

            return RedirectToAction("Index");
        }
    }
}
