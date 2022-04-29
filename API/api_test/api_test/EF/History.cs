using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class History
    {
        public int IdArticles { get; set; }
        public int IdUser { get; set; }
        public DateTime DatetimeSeen { get; set; }

        public virtual Article IdArticlesNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
