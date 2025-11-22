using GymManagmentDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.DataSeed
{
    public static class IDentityDbContextSeeding
    {

        public static bool SeedData(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {

            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return false;

                if(!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new() {Name = "SuperAdmin"},
                        new() {Name = "Admin"},
                    };

                    foreach (var role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }

                    }
                }

                if(!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Zhraan",
                        UserName = "AhmedZhraan",
                        Email = "AhmedZhraan@gmail.com",
                        PhoneNumber = "01052634158"
                    };
                    userManager.CreateAsync(MainAdmin , "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Zhraan",
                        UserName = "MohamedZhraan",
                        Email = "MohamedZhraan@gmail.com",
                        PhoneNumber = "01052634155"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }


        }

    }
}
