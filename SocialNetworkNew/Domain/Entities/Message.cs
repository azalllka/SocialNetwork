using System;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public IdentityUser? Sender { get; set; }
        public string ReceiverId { get; set; } = null!;
        public IdentityUser? Receiver { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
} 