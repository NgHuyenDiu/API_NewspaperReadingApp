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

        public List<int> listCategory { get; set; }
    }

    public class ArticlesModelEdit
    {
        public string Title { get; set; }
        public string ContentArticles { get; set; }
      
        public string Status { get; set; }
        public string Image { get; set; }

        public List<int> listCategory { get; set; }
    }

    public class ArticlesModelView
    {
        public int IdArticles { get; set; }
        public string Title { get; set; }
        public string ContentArticles { get; set; }
        public int IdUser { get; set; }
        public string Status { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Image { get; set; }
        public int? Views { get; set; }

        public List<int> listCategory { get; set; }

        public UserView user { get; set; }
    }

    public class IDArticles
    {
        public int IdArticles { get; set; }
    }
}
