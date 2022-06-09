using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Model
{
    public class HistoryModel
    {
        public int IdArticles { get; set; }
        public int IdUser { get; set; }

    }

    public class HistoryDelete
    {
        public int IdUser { get; set; }
        public int IdArticles { get; set; }
       
        public DateTime DatetimeSeen { get; set; }
    }
}
