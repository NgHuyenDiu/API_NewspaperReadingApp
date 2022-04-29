using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class AuthorFavorite
    {
        public int IdFavorite { get; set; }
        public int IdAuthor { get; set; }
        public int IdUser { get; set; }

        public virtual User IdAuthorNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
