using System;
using System.Linq;
using Catalog.Data;
using MongoDB.Driver;


namespace JsonReport
{
    public class JsonClient
    {
        static void Main(string[] args)
        {
            var db = new Catalog.Data.CatalogContext();

            CreateJsonObject(db);
        }

        private static void CreateJsonObject(CatalogContext db)
        {
            using (db)
            {
                var productReport = db.Sales
                    .Include("Product")
                    .Include("Product.Vendor")
                    .Select(x => new
                    {
                        x.Sum,
                        x.Quantity,
                        x.ProductID,
                        x.Product.ProductName,
                        x.Product.Vendor.Name
                    }
                            )
                    .GroupBy(x => x.ProductID);

                int count = 0;
                foreach (var item in productReport)
                {
                    Console.WriteLine("ID: {0} Name: {1} Vendor: {2} Quantity: {3} Sum: {4}",
                        item.Key,
                        item.First().ProductName,
                        item.First().Name,
                        item.Sum(x => x.Quantity),
                        item.Sum(x => x.Sum)
                        );

                    ProductReport currentReport = new ProductReport(item.Key, item.First().ProductName, item.First().Name, 
                                                                    Convert.ToDouble(item.Sum(x => x.Quantity)), item.Sum(x => x.Sum));

                    InsertIntoMongoProductReport(currentReport, count);
                    count++;
                }
            }
        }

        private static void InsertIntoMongoProductReport(ProductReport currentReport, int check)
        {
            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();
            var productReporstDb = mongoServer.GetDatabase("Supermarket");

            var collection = productReporstDb.GetCollection<ProductReport>("ProductReports");

            if (check == 0)
            {
                collection.RemoveAll();
            }
            else
            {
                collection.Save(currentReport);
            }
        }

        public static void InsertIntoMongoVendorReport(VendorReport currentReport, int check)
        {
            var mongoClient = new MongoClient("mongodb://localhost/");
            var mongoServer = mongoClient.GetServer();
            var productReporstDb = mongoServer.GetDatabase("Supermarket");

            var collection = productReporstDb.GetCollection<ProductReport>("VendorReports");

            if (check == 0)
            {
                collection.RemoveAll();
            }
            else
            {
                collection.Save(currentReport);
            }
        }
    }
}
