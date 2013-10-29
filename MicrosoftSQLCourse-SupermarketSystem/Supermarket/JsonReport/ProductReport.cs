using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JsonReport
{
    public class ProductReport
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int ProductReportID { get; set; }
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string Vendor { get; set; }
        public double TotalQuantitySold { get; set; }
        public decimal TotalIncoms { get; set; }
        public ProductReport()
        {
        }
        public ProductReport(int id, string name, string vendor, double totalQ, decimal totalIncoms)
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