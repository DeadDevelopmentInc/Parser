using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Users
{
    internal class PostgreUser
    {
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public List<string> pass { get; set; }
        public Date stWD { get; set; }
        public string ofRoom { get; set; }
        public string ofAdd { get; set; }
        public string sphere { get; set; }
        public string div { get; set; }
        public string dep { get; set; }
        public string sec { get; set; }
        public string pos { get; set; }
        public string univer { get; set; }
        public List<Date> vacation { get; set; }
        public string edu { get; set; }
        public Date bith { get; set; }
        public string email { get; set; }

    }
}
