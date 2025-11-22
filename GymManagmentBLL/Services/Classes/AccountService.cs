using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels.LoginViewModel;
using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
           var User = _userManager.FindByEmailAsync(loginViewModel.Email).Result;

            if(User is null) return null;

            var IsPasswordVaild = _userManager.CheckPasswordAsync(User, loginViewModel.password).Result;

            return IsPasswordVaild ? User : null;
        }
    }
}
