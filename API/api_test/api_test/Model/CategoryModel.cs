using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Model
{
    public class CategoryModel
    {
        
        public string Title { get; set; }
    }

    public class CategoryView
    {
        public int IdCategory { get; set; }
        public string Title { get; set; }

        public List<IDArticles> listIDArticles { get; set; }
    }
}
