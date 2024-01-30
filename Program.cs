using AspNetCoreHero.ToastNotification;

using final.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using final.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();



builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlCon"));
});
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 1;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.Cookie.Name = "final";
        opt.ExpireTimeSpan = TimeSpan.FromDays(3);
        opt.LoginPath = "/Home/Login";
        opt.LogoutPath = "/Home/Logout";
        opt.AccessDeniedPath = "/Home/AccessDenied";
        opt.SlidingExpiration = false;
    });
// Add services to the container.
// Add services to the container.
// Add services to the container.

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapHub<ChatHub>("/chatHub");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
