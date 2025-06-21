using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; } = null!;
        public string? Bio { get; set; }
        public int Gender { get; set; } = 0; // 0 - не указано, 1 - мужской, 2 - женский
        public int Relationship { get; set; } = 0; // 0 - не указано, 1 - холост, 2 - в отношениях, 3 - женат, 4 - помолвлен
        public string? AvatarUrl { get; set; }
    }
} 