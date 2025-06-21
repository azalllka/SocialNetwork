using SocialNetwork.Persistence.Contexts;
using SocialNetwork.Application.Extensions;
using System.Text.Json.Serialization;
using SocialNetwork.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Persistence.Repositories;
using SocialNetwork.Infrastructure.Services;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Application.Interfaces.Services;
using SocialNetwork.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Регистрируем ApplicationDbContext напрямую для миграций
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
        builder.Services.AddApplicationLayer();
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Настройка аутентификации
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/Login";
        });

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Auth}/{action=Login}/{id?}");

        app.Run();
    }
}
