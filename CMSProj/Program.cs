using Microsoft.AspNetCore;

using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ContentDatabase.DIExtensions;
using CMSProj.DataLayer.ServiceRegistration;
using CMSProj;
using CMSProj.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CMSProj.Areas.Identity.Data;
using CMSProj.SubSystems.Publishing.Registration;
using System.IO;
using CMSProj.DataLayer;
using CMSProj.Controllers;
using CMSProj.SubSystems.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x => {
    x.Limits.MaxConcurrentConnections = 5;
    x.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
    //since we swallow literally everything and regex it, this is a possible ddos vector.
    x.Limits.MaxRequestLineSize = 2043;
});
builder.Configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "runtimevars.json"), true, true);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AdminsPolicy", pol => pol.RequireRole("Admin"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddControllersWithViews();
builder.Services.AddExistingRoutesHandler();
builder.Services.AddPageManagement();
builder.Services.AddContentContext(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IAdminAssetProvider, AdminAssetProvider>();

builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityContextConnection")));

builder.Services.AddDefaultIdentity<AdminUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(opts => {
    opts.LoginPath = "/Admin/Home";
    opts.AccessDeniedPath = "/AccessDenied";
    opts.SlidingExpiration = true;
    opts.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddDynmicRouteServices();
builder.Services.AddRoutesServices();
builder.Services.AddPageServices();
builder.Services.AddCmsSeeders();
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorizationPolicyEvaluator();
builder.Services.AddPageCounters(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "public")),
    RequestPath = "/public"
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

//
// Adminpages redirection, Tabled for now.
//
//app.Use((ctx, next) =>
//{
//    const string AdminRoot = "/Admin";
//
//      bool isExcluded =
//          path.StartsWith(areaRoot, StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/api", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/css", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/js", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/images", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/signin-oidc", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/signout-callback-oidc", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/account/logout", StringComparison.OrdinalIgnoreCase) ||
//          path.StartsWith("/health", StringComparison.OrdinalIgnoreCase);
//
//    bool adminPagesRedirect = false;
//    if (ctx.Request.Path.HasValue && !ctx.Request.Path.Value.StartsWith("Identity") ||
//                                    !ctx.Request.Path.Value.EndsWith("/RequestView"))
//        adminPagesRedirect = true;
//
//    if(ctx.User.IsInRole("Admin") 
//    && (ctx.User?.Identity?.IsAuthenticated ?? false)
//    && adminPagesRedirect)
//
//        
//    return next();
//});
//

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapDynamicControllerRoute<RouteTransformer>("{**slug}");
//await app.Services.SeedRolesAndAdminAsync();
app.Run();

