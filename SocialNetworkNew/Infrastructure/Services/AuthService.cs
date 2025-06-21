using Microsoft.AspNetCore.Identity;
using SocialNetwork.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailService _emailService;

    public AuthService
        (UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, 
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public async Task<bool> RegisterAsync(string firstname, string lastname, string email, string password)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };
         var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Identity error: {error.Description}");
            }
            return false;
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        var subject = "Регистрация прошла успешно";
        var body = $"<h1>Здравствуйте, {firstname}!</h1>" +
                         "<p>Вы успешно зарегистрировались на нашем сайте.</p>" +
                         $"<p>Login: {email}</p>" +
                         $"<p>Password: {password}</p>";
        await _emailService.SendEmailAsync(email, subject, body);
       

        return true;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        return result.Succeeded ? true :
        false;
    }
}
