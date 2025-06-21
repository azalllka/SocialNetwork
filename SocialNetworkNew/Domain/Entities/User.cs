using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Entities.Enums;

namespace SocialNetwork.Domain.Entities;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Img { get; set; }
    public string? Bio { get; set; }
    public Gender Gender { get; set; }
    public Relationship Relationship { get; set; }
    public Enable TwoFactorAuthentication { get; set; }
}
