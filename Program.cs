using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bicycle.Data;
using Bicycle.Data.Repositories;
using Bicycle.Models;
using Bicycle.Services;
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

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.Lockout.AllowedForNewUsers = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>();
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<DBSeeder>();
        
        var awsOptions = builder.Configuration.GetSection("AWS");
        var credentials = new BasicAWSCredentials(awsOptions["AccessKey"], awsOptions["SecretKey"]);

        builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(credentials, Amazon.RegionEndpoint.APNortheast2));
        builder.Services.AddSingleton<AmazonS3Service>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<UserManager<IdentityUser>>();
        builder.Services.AddScoped<SignInManager<IdentityUser>>();

        builder.Services.AddSignalR().AddHubOptions<MessageHub>(options =>
        {
            options.EnableDetailedErrors = true;
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        });

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
        app.MapHub<MessageHub>("/chat");

        app.Run();
    }
}
