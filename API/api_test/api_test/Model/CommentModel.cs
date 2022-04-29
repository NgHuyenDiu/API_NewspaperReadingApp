using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Model
{
    public class CommentModel
    {
        public int IdUser { get; set; }
        public int IdArticles { get; set; }
        public string ContentComment { get; set; }
    }
    public class CommentModelEdit
    {
        public string ContentComment { get; set; }
    }
}
