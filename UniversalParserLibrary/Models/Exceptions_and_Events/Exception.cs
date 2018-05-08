using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Exceptions_and_Events
{
    internal class Exception : Event
    {
        public Exception(string process, string type, string message, string email) : base(process, type, message)
        {
            this.email = email;
            Console.WriteLine(ToString());
        }

        public Exception(string process, string type, string message) : base(process, type, message) => Console.WriteLine(ToString());

        public Exception(string process, string type, string message, int value) : base(process, type, message)
        {
            this.value = value.ToString();
            Console.WriteLine(ToString());
        }

        public override string ToString() => this.ToJson();
    }
}
