using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class Article
    {
        public Article()
        {
            Comments = new HashSet<Comment>();
            Histories = new HashSet<History>();
            Qltlbvs = new HashSet<Qltlbv>();
        }

        public int IdArticles { get; set; }
        public string Title { get; set; }
        public string ContentArticles { get; set; }
        public int IdUser { get; set; }
        public string Status { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Image { get; set; }
        public int? Views { get; set; }
        public int? TrangThaiXoa { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<Qltlbv> Qltlbvs { get; set; }
    }
}
