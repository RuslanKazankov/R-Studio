using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using R_StudioAPI.Models;

namespace R_StudioAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public DbSet<Video> Video { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.MyPosts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithOne(u => u.Favourite)
                .HasForeignKey<Favourite>(f => f.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasMany(u => u.History)
                .WithMany();

            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedVideos)
                .WithMany();

            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedPosts)
                .WithMany();

            modelBuilder.Entity<Video>()
                .HasMany(v => v.Actors)
                .WithMany();

            modelBuilder.Entity<Favourite>()
                .HasMany(f => f.Users)
                .WithMany();

            modelBuilder.Entity<Favourite>()
                .HasMany(f => f.Videos)
                .WithMany();

            modelBuilder.Entity<Favourite>()
                .HasMany(f => f.Posts)
                .WithMany();

            List<IdentityRole<long>> roles = new(){
                new IdentityRole<long>
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = 2
                },
                new IdentityRole<long>
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR",
                    Id = 3
                },
                new IdentityRole<long>
                {
                    Name = "Actor",
                    NormalizedName = "ACTOR",
                    Id = 4
                },
                new IdentityRole<long>
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = 1
                }
            };
            modelBuilder.Entity<IdentityRole<long>>().HasData(roles);
        }
    }
}
