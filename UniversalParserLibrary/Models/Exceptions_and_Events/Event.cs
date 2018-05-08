using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Exceptions_and_Events
{
    [DataContract]
    internal abstract class Event
    {
        [DataMember]
        public string datetime { get; } = DateTime.Now.Date.ToString();
        [DataMember]
        public string process { get; set; } = null;
        [DataMember]
        public string type { get; set; } = null;
        [DataMember]
        public string message { get; set; } = null;
        [DataMember]
        public string value { get; set; } = null;
        [DataMember]
        public string email { get; set; } = null;
        
        public Event(string process, string type, string message)
        {
            this.process = process;
            this.type = type;
            this.message = message;
        }
    }
}
