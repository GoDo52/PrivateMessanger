using Microsoft.EntityFrameworkCore;
using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<UserChatRole> UserChatRoles { get; set; }
        public DbSet<AdministrationRole> AdministrationRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Default administration roles
            modelBuilder.Entity<AdministrationRole>().HasData(
                new AdministrationRole { Id = 1, Name = "User"},
                new AdministrationRole { Id = 2, Name = "Manager"},
                new AdministrationRole { Id = 3, Name = "Administrator"}
            );

            // Default chat roles
            modelBuilder.Entity<UserChatRole>().HasData(
                new UserChatRole { Id = 1, Name = "Member"},
                new UserChatRole { Id = 2, Name = "Admin"},
                new UserChatRole { Id = 3, Name = "SuperAdmin"}
            );

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.UserChatRole)
                .WithMany()
                .HasForeignKey(uc => uc.RoleId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Tag)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName);

            modelBuilder.Entity<UserChat>()
                .HasKey(uc => new { uc.UserId, uc.ChatId });

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UserChats)
                .HasForeignKey(uc => uc.ChatId);
        }
    }
}
