using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Exceptions_and_Events
{
    internal class Info : Event
    {

        public Info(string process, string type, string message, string email, int value) : base(process, type, message)
        {
            this.email = email;
            this.value = value.ToString();
            Console.WriteLine(ToString());
        }

        public Info(string process, string type, string message, string email) : base(process, type, message)
        {
            this.email = email;
            Console.WriteLine(ToString());
        }

        public Info(string process, string type, string message, int value) : base(process, type, message)
        {
            this.value = value.ToString();
            Console.WriteLine(ToString());
        }

        public override string ToString() => this.ToJson();
    }
}
