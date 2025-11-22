using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.LoginViewModel;
using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            this._accountService = accountService;
            this._signInManager = signInManager;
        }

        // Login   

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _accountService.ValidateUser(model);
            if (user is null)
            {
                return View(model);
            }

            var Result = _signInManager.PasswordSignInAsync(user , model.password , model.rememberMe , false).Result;

            if (Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Is Not Allowed");
            if(Result.IsLockedOut) 
                ModelState.AddModelError("InvalidLogin", "Your Account Is  LoackedOut");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(model);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        // Logout
        // Access eanied
    }
}
