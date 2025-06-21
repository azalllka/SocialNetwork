using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Persistence.Contexts;
using System.Security.Claims;
using SocialNetwork.Models.Profile;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ProfileController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string friendId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == friendId) 
                return Json(new { success = false, message = "Нельзя подписаться на себя" });

            var exists = await _dbContext.Friendships.AnyAsync(f => f.UserId == userId && f.FriendId == friendId);
            if (exists) 
                return Json(new { success = false, message = "Уже подписаны или заявка отправлена" });

            var friendship = new Domain.Entities.Friendship
            {
                UserId = userId,
                FriendId = friendId,
                IsAccepted = false // или true, если сразу друзья
            };
            _dbContext.Friendships.Add(friendship);
            await _dbContext.SaveChangesAsync();

            return Json(new { success = true, message = "Запрос отправлен" });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();

            var currentUserId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            bool isFriend = false;
            if (currentUserId != null && currentUserId != id)
            {
                isFriend = await _dbContext.Friendships.AnyAsync(f => f.UserId == currentUserId && f.FriendId == id && f.IsAccepted);
            }

            var model = new ProfileViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Name = user.UserName ?? "",
                Img = "",
                Bio = "",
                IsFriend = isFriend
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FriendRequests()
        {
            var currentUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var requests = await _dbContext.Friendships
                .Where(f => f.FriendId == currentUserId && !f.IsAccepted)
                .Include(f => f.User) // если навигационное свойство User есть
                .ToListAsync();
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriend(string userId)
        {
            var currentUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var friendship = await _dbContext.Friendships
                .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == currentUserId && !f.IsAccepted);
            if (friendship != null)
            {
                friendship.IsAccepted = true;
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Pages", "Home");
        }
    }
} 