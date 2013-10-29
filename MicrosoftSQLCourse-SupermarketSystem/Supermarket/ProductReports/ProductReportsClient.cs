using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ProductReports
{
    public class ProductReportsClient
    {
        static void Main(string[] args)
        {
            //var mongoClient = new MongoClient("mongodb://localhost/");
            //var mongoServer = mongoClient.GetServer();
            //var productReporstDb = mongoServer.GetDatabase("ProductReports");
            //AddToDB(MongoCollection <BsonDocument> clicks);
        }

        public static void AddToDB(MongoCollection storageTarget)
        {
            String[] allfiles = System.IO.Directory.GetFiles(@"..\..\..\Reports\", "*.*", System.IO.SearchOption.AllDirectories);
            
            foreach (var item in allfiles)
            {
                string text = System.IO.File.ReadAllText(item);
                storageTarget.Insert(text);
            }
        }
    }
}