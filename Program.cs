using IT_Helpdesk_System.Data;
using IT_Helpdesk_System.Data.Seed;
using IT_Helpdesk_System.Models;
using IT_Helpdesk_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================= DATABASE =======================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// ======================= IDENTITY + ROLES =======================
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ======================= IDENTITY UI DEPENDENCY =======================
// Prevents Register page crash when email sender is required
builder.Services.AddSingleton<IEmailSender, NullEmailSender>();


// ======================= MVC + RAZOR =======================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // REQUIRED for Identity UI


var app = builder.Build();


// ======================= MIDDLEWARE =======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // MUST come before authorization
app.UseAuthorization();


// ======================= ROUTING =======================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // REQUIRED for Identity


// ======================= SEED ROLES & ADMIN =======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAdminAsync(userManager);
}

app.Run();
