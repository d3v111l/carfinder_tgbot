using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carfinder_tgbotcon.ClassModels
{
    public class SearchResult
    {
        public List<string> ids { get; set; }
        public int count { get; set; }
        public int lastId { get; set; }
    }

    public class Root
    {
        public SearchResult search_Result { get; set; }
    }
}
