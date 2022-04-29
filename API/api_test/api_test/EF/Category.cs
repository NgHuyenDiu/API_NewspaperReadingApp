using System;
using System.Collections.Generic;

#nullable disable

namespace api_test.EF
{
    public partial class Category
    {
        public Category()
        {
            Qltlbvs = new HashSet<Qltlbv>();
        }

        public int IdCategory { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Qltlbv> Qltlbvs { get; set; }
    }
}
