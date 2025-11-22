using GymManagmentBLL;
using GymManagmentBLL.Services.AttachmentService;
using GymManagmentBLL.Services.Classes;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentDAL.Data.DataSeed;
using GymManagmentDAL.Data.GymDBContext;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Classes;
using GymManagmentDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddScoped<ISessionRepository , SessionRepository>();
            builder.Services.AddScoped<IMemberService , MemberService>();
            builder.Services.AddScoped<IAnalyticsService , AnayticsService>();
            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddScoped<IPlanService , PlanService>();
            builder.Services.AddScoped<ISessionService , SessionService>();
            builder.Services.AddScoped<ITrainerService , TrainerService>();
            builder.Services.AddScoped<IAttachmentService , AttachmentService>();
            builder.Services.AddScoped<IAccountService , AccountService>();


            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            builder.Services.AddIdentity<ApplicationUser , IdentityRole>(congig =>
            {
                congig.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            
            var app = builder.Build();

            #region DataSeeding
            using var scope = app.Services.CreateScope();

            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigration = gymDbContext.Database.GetPendingMigrations();

            if(pendingMigration?.Any() ?? false)
            {
                gymDbContext.Database.Migrate();
            }
            GymDataSeeding.SeedDate(gymDbContext);

            IDentityDbContextSeeding.SeedData(roleManager, userManager);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
