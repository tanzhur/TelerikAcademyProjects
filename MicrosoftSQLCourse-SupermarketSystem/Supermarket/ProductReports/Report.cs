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
    public class Report
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }
        public string Vendor { get; set; }
        public double TotalQuantitySold { get; set; }
        public decimal TotalIncoms { get; set; }

        [BsonConstructor]
        public Report(int id, string name, string vendor, double totalQ, decimal totalIncoms)
        {
            this.Id = new ObjectId();
            this.ProductId = id;
            this.Name = name;
            this.Vendor = vendor;
            this.TotalQuantitySold = totalQ;
            this.TotalIncoms = totalIncoms;
        }
    }
}
