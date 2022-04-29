using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Model
{
    public class ArticlesModel
    {
        public string Title { get; set; }
        public string ContentArticles { get; set; }
        public int IdUser { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
    }

    public class ArticlesModelEdit
    {
        public string Title { get; set; }
        public string ContentArticles { get; set; }
      
        public string Status { get; set; }
        public string Image { get; set; }
    }
}
