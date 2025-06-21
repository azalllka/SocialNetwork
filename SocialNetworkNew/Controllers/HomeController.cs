using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Features.Comments.Query.GetByPostId;
using SocialNetwork.Application.Features.Posts.Query.GetByUserPageId;
using SocialNetwork.Models;
using SocialNetwork.Models.Home;
using SocialNetwork.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Entities.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, IMediator mediator, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var userPageId = 1;
            var query = new GetByUserPageIdQuery(userPageId);
            var query2 = new GetByPostIdQuery(query.UserPageId);

            var viewModel = new HomeVM
            {
                Feeds = await _mediator.Send(query),
            };
            return View(viewModel);
        }

        public IActionResult Messages() { return View(); }
        public IActionResult Event() { return View(); }
        public IActionResult Groups() { return View(); }
        [HttpGet]
        public async Task<IActionResult> Pages()
        {
            var users = await _dbContext.Users.ToListAsync();
            var currentUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var friends = await _dbContext.Friendships
                .Where(f => f.UserId == currentUserId && f.IsAccepted)
                .Select(f => f.FriendId)
                .ToListAsync();
            var following = await _dbContext.Friendships
                .Where(f => f.UserId == currentUserId && !f.IsAccepted)
                .Select(f => f.FriendId)
                .ToListAsync();
            var incomingRequests = await _dbContext.Friendships
                .Where(f => f.FriendId == currentUserId && !f.IsAccepted)
                .Include(f => f.User)
                .ToListAsync();
            ViewBag.Friends = friends;
            ViewBag.Following = following;
            ViewBag.CurrentUserId = currentUserId;
            ViewBag.IncomingRequests = incomingRequests;
            return View(users);
        }
        public IActionResult Market() { return View(); }
        public IActionResult Event2() { return View(); }
        public IActionResult Groups2() { return View(); }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound();

            // Получаем или создаем профиль пользователя
            var profile = await _dbContext.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = user.Id,
                    Bio = "",
                    Gender = 0,
                    Relationship = 0
                };
                _dbContext.UserProfiles.Add(profile);
                await _dbContext.SaveChangesAsync();
            }

            var model = new UserSettingsViewModel
            {
                Username = user.UserName ?? "",
                Email = user.Email ?? "",
                Bio = profile.Bio ?? "",
                Gender = profile.Gender,
                Relationship = profile.Relationship
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(UserSettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return View(model);
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return View(model);
            }

            // Обновляем основную информацию пользователя
            user.UserName = model.Username;

            // Получаем или создаем профиль пользователя
            var profile = await _dbContext.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = user.Id
                };
                _dbContext.UserProfiles.Add(profile);
            }

            // Обновляем профиль
            profile.Bio = model.Bio;
            profile.Gender = model.Gender;
            profile.Relationship = model.Relationship;

            await _dbContext.SaveChangesAsync();

            ViewBag.Message = "Настройки успешно сохранены!";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Messages(string? withUserId = null)
        {
            var currentUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            // Получаем список друзей
            var friendsIds = await _dbContext.Friendships
                .Where(f => (f.UserId == currentUserId || f.FriendId == currentUserId) && f.IsAccepted)
                .Select(f => f.UserId == currentUserId ? f.FriendId : f.UserId)
                .Distinct()
                .ToListAsync();
            var friends = await _dbContext.Users.Where(u => friendsIds.Contains(u.Id)).ToListAsync();
            // История сообщений с выбранным другом
            List<Message> messages = new();
            IdentityUser? chatUser = null;
            if (!string.IsNullOrEmpty(withUserId) && friendsIds.Contains(withUserId))
            {
                messages = await _dbContext.Messages
                    .Where(m => (m.SenderId == currentUserId && m.ReceiverId == withUserId) || (m.SenderId == withUserId && m.ReceiverId == currentUserId))
                    .OrderBy(m => m.SentAt)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .ToListAsync();
                chatUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == withUserId);
            }
            ViewBag.Friends = friends;
            ViewBag.ChatUser = chatUser;
            ViewBag.Messages = messages;
            ViewBag.CurrentUserId = currentUserId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string text)
        {
            var currentUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(receiverId))
                return RedirectToAction("Messages", new { withUserId = receiverId });
            // Проверяем, что это друг
            var isFriend = await _dbContext.Friendships.AnyAsync(f =>
                ((f.UserId == currentUserId && f.FriendId == receiverId) || (f.UserId == receiverId && f.FriendId == currentUserId))
                && f.IsAccepted);
            if (!isFriend) return Forbid();
            var message = new Message
            {
                SenderId = currentUserId!,
                ReceiverId = receiverId,
                Text = text,
                SentAt = DateTime.UtcNow
            };
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Messages", new { withUserId = receiverId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
