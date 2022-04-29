using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class Qltlbv
    {
        public int IdQl { get; set; }
        public int IdArticles { get; set; }
        public int IdCategory { get; set; }

        public virtual Article IdArticlesNavigation { get; set; }
        public virtual Category IdCategoryNavigation { get; set; }
    }
}
