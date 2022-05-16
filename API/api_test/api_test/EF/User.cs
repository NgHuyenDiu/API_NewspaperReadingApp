using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class User
    {
        public User()
        {
            AuthorFavoriteIdAuthorNavigations = new HashSet<AuthorFavorite>();
            AuthorFavoriteIdUserNavigations = new HashSet<AuthorFavorite>();
            Comments = new HashSet<Comment>();
            Histories = new HashSet<History>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int IdUser { get; set; }
        public string Name { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avata { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<AuthorFavorite> AuthorFavoriteIdAuthorNavigations { get; set; }
        public virtual ICollection<AuthorFavorite> AuthorFavoriteIdUserNavigations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
