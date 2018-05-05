using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Helpers
{
    internal static class PrivateDictionary
    {
        const string connectionAdmin = @"mongodb://admin:78564523@ds054128.mlab.com:54128/technologies_db";
        private static List<ModelSkill> dictionary;
        
        static PrivateDictionary()
        {
            MongoClient client = new MongoClient(connectionAdmin);
            IMongoDatabase database = client.GetDatabase("technologies_db");
            IMongoCollection<BsonDocument> collection = 
                database.GetCollection<BsonDocument>("skills");
            dictionary = BsonSerializer.Deserialize<List<ModelSkill>>(collection.ToBsonDocument());

        }
    }
}
