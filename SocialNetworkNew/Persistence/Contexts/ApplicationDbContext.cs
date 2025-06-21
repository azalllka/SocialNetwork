using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SocialNetwork.Domain.Entities;


namespace SocialNetwork.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Notify> Notify { get; set; }
        public DbSet<Privacy> Privacy { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public static ApplicationDbContext Create(DbContextOptions<ApplicationDbContext> options)
            => new ApplicationDbContext(options);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PostsConfiguration());
            modelBuilder.ApplyConfiguration(new EventsConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new NotifyConfiguration());
            modelBuilder.ApplyConfiguration(new PrivacyConfiguration());
            modelBuilder.ApplyConfiguration(new SocialLinksConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsConfiguration());
            modelBuilder.Entity<Friendship>()
                .HasOne<IdentityUser>(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
