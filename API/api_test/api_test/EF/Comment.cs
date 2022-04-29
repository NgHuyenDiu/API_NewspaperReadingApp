using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class Comment
    {
        public int IdComment { get; set; }
        public int IdUser { get; set; }
        public int IdArticles { get; set; }
        public string ContentComment { get; set; }

        public virtual Article IdArticlesNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
