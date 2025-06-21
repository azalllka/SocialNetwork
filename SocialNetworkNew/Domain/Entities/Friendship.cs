using System;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Domain.Entities
{
    public class Friendship
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!; // кто подписался
        public IdentityUser? User { get; set; } // навигационное свойство
        public string FriendId { get; set; } = null!; // на кого подписался
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsAccepted { get; set; } // true - друзья, false - заявка/подписка
    }
} 