using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FinatTestTechnique2.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")));

//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add(new AuthorizeFilter());
//}).AddRazorPagesOptions(options =>
//{
//    options.Conventions.AuthorizePage("/Home/Privacy");
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//    options.LogoutPath = "/Account/Logout";
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//    options.SlidingExpiration = true;
//    options.Events.OnSignedIn = context =>
//    {
//        context.Response.Redirect("/Home/Privacy");
//        return Task.CompletedTask;
//    };
//});

var app = builder.Build();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
