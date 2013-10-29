using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace JsonReport
{
    public class VendorReport
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Vendor { get; set; }
        public string Month { get; set; }
        public decimal Expenses { get; set; }

        public VendorReport(string vendor, string month, decimal expenses)
        {
            this.Vendor = vendor;
            this.Month = month;
            this.Expenses = expenses;
        }
    }
}
