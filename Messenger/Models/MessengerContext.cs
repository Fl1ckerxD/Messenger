using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Messenger.Models
{
    public partial class MessengerContext : DbContext
    {
        public MessengerContext()
        {
        }

        public MessengerContext(DbContextOptions<MessengerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<MessageUser> MessageUsers { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["MDF"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.Theme).HasMaxLength(50);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Chat_Department");

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Chats)
                    .UsingEntity<Dictionary<string, object>>(
                        "ChatUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").HasConstraintName("FK_ChatUser_User"),
                        r => r.HasOne<Chat>().WithMany().HasForeignKey("ChatId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ChatUser_Chat"),
                        j =>
                        {
                            j.HasKey("ChatId", "UserId");

                            j.ToTable("ChatUser");
                        });
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Title).HasMaxLength(150);
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileData).HasColumnName("File_data");

                entity.Property(e => e.FileExtension).HasColumnName("File_extension");

                entity.Property(e => e.FileLength).HasColumnName("File_length");

                entity.Property(e => e.FileName)
                    .HasMaxLength(100)
                    .HasColumnName("File_name");

                entity.Property(e => e.MessageId).HasColumnName("Message_id");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_Files_Message");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Content).HasMaxLength(200);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasDefaultValueSql("((20))");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Chat");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_User");
            });

            modelBuilder.Entity<MessageUser>(entity =>
            {
                entity.HasKey(e => new { e.MessageId, e.UserId });

                entity.ToTable("MessageUser");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageUsers)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_MessageUser_Message");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MessageUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MessageUser_User");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasMany(d => d.Departments)
                    .WithMany(p => p.Posts)
                    .UsingEntity<Dictionary<string, object>>(
                        "DepartmentPost",
                        l => l.HasOne<Department>().WithMany().HasForeignKey("DepartmentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_DepartmentPost_Department"),
                        r => r.HasOne<Post>().WithMany().HasForeignKey("PostId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_DepartmentPost_Post"),
                        j =>
                        {
                            j.HasKey("PostId", "DepartmentId");

                            j.ToTable("DepartmentPost");
                        });
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Title).HasMaxLength(15);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.LastName).HasMaxLength(30);

                entity.Property(e => e.Login).HasMaxLength(25);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(35);

                entity.Property(e => e.Phone).HasMaxLength(12);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_User_Department");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_User_Post");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_User_Status");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK_User_UserType");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.Title).HasMaxLength(15);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
