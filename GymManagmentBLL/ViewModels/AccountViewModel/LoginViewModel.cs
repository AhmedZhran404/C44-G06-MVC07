using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.LoginViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Is Requied")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password Is Requied")]
        [DataType(DataType.Password)] // ****
        public string password { get; set; } = null!;

        public bool rememberMe { get; set; }
    }
}
