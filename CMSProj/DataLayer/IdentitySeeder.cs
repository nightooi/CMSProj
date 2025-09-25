using CMSProj.Areas.Identity.Data;

using Microsoft.AspNetCore.Identity;

namespace CMSProj.DataLayer
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AdminUser>>();

            // roles
            foreach (var role in new[] { "Admin" })
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            
            // admin user
            var adminEmail = "barrrte97@gmail.com";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new AdminUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var t = userMgr.CreateAsync(admin, "Blasphemy29!");
                await t;
                if(t.IsCompleted || t.IsFaulted)
                {
                    Console.WriteLine(t.Result.Errors.FirstOrDefault()?.Description ?? "not desc");
                    Console.ReadKey();
                }
                var usr = await userMgr.FindByEmailAsync(adminEmail);

                Console.WriteLine(usr.Id);
                await userMgr.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
