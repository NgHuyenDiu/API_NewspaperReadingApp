using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace api_test.EF
{
    public partial class NewspaperReadingAppContext : DbContext
    {
        public NewspaperReadingAppContext()
        {
        }

        public NewspaperReadingAppContext(DbContextOptions<NewspaperReadingAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<AuthorFavorite> AuthorFavorites { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Qltlbv> Qltlbvs { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=MyDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.IdArticles);

                entity.Property(e => e.IdArticles)
                    .ValueGeneratedNever()
                    .HasColumnName("id_articles");

                entity.Property(e => e.ContentArticles)
                    .IsRequired()
                    .HasColumnName("content_articles");

                entity.Property(e => e.DateSubmitted)
                    .HasColumnType("date")
                    .HasColumnName("date_submitted")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Image)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("title");

                entity.Property(e => e.TrangThaiXoa)
                    .HasColumnName("trangThaiXoa")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Views).HasColumnName("views");
            });

            modelBuilder.Entity<AuthorFavorite>(entity =>
            {
                entity.HasKey(e => e.IdFavorite);

                entity.ToTable("Author_favorite");

                entity.Property(e => e.IdFavorite)
                    .ValueGeneratedNever()
                    .HasColumnName("id_favorite");

                entity.Property(e => e.IdAuthor).HasColumnName("id_author");

                entity.Property(e => e.IdUser).HasColumnName("id_User");

                entity.HasOne(d => d.IdAuthorNavigation)
                    .WithMany(p => p.AuthorFavoriteIdAuthorNavigations)
                    .HasForeignKey(d => d.IdAuthor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Author_favorite_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.AuthorFavoriteIdUserNavigations)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Author_favorite_User1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);

                entity.ToTable("Category");

                entity.Property(e => e.IdCategory)
                    .ValueGeneratedNever()
                    .HasColumnName("id_category");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComment);

                entity.ToTable("Comment");

                entity.Property(e => e.IdComment)
                    .ValueGeneratedNever()
                    .HasColumnName("id_comment");

                entity.Property(e => e.ContentComment)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("content_comment");

                entity.Property(e => e.IdArticles).HasColumnName("id_articles");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.HasOne(d => d.IdArticlesNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdArticles)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Articles");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => new { e.IdArticles, e.IdUser, e.DatetimeSeen });

                entity.ToTable("History");

                entity.Property(e => e.IdArticles).HasColumnName("id_articles");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.DatetimeSeen)
                    .HasColumnType("datetime")
                    .HasColumnName("datetime_seen")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdArticlesNavigation)
                    .WithMany(p => p.Histories)
                    .HasForeignKey(d => d.IdArticles)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_Articles");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Histories)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_User");
            });

            modelBuilder.Entity<Qltlbv>(entity =>
            {
                entity.HasKey(e => e.IdQl)
                    .HasName("PK_Management_Articles_Category");

                entity.ToTable("QLTLBV");

                entity.Property(e => e.IdQl)
                    .ValueGeneratedNever()
                    .HasColumnName("id_QL");

                entity.Property(e => e.IdArticles).HasColumnName("id_articles");

                entity.Property(e => e.IdCategory).HasColumnName("id_category");

                entity.HasOne(d => d.IdArticlesNavigation)
                    .WithMany(p => p.Qltlbvs)
                    .HasForeignKey(d => d.IdArticles)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QLTLBV_Articles");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Qltlbvs)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QLTLBV_Category");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshToken_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.Property(e => e.IdUser)
                    .ValueGeneratedNever()
                    .HasColumnName("id_user");

                entity.Property(e => e.Avata)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("avata");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
