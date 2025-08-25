using CMSProj.Controllers;

using Microsoft.AspNetCore;

using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ContentDatabase.DIExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x => {
    x.Limits.MaxConcurrentConnections = 5;
    x.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
    //since we swallow literally everything and regex it, this is a possible ddos vector.
    x.Limits.MaxRequestLineSize = 2043;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddExistingRoutesHandler();
builder.Services.RegisterDynmicServices();
builder.Services.AddContentContext(builder.Configuration.GetConnectionString("Default")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapDynamicControllerRoute<RouteTransformer>("{**slug}");
app.Run();

