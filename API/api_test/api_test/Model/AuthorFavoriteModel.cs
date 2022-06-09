using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Model
{
    public class AuthorFavoriteModel
    {
        public int IdAuthor { get; set; }
        public int IdUser { get; set; }
    }

    public class CountNumber
    {
        public int number_of_authors_I_follow { get; set; }

        public int number_of_people_watching { get; set; }
    }
}
