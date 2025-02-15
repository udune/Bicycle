using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bicycle.Data;
using Bicycle.Data.Repositories;
using Microsoft.Extensions.FileProviders;
using MySql.Data.MySqlClient;

namespace Bicycle;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionStringBuilder = new MySqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionStringBuilder.ConnectionString, new MySqlServerVersion(new Version(8, 0, 40))));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<DBSeeder>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

        var app = builder.Build();
        
        app.Services.CreateScope().ServiceProvider.GetRequiredService<DBSeeder>().SeedDatabase().Wait();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
