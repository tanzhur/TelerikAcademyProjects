using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using JsonReport;
using SQLiteModel;
namespace VendorsTotalReport
{
    public class Class1
    {
        public static void Main()
        {
            using (var sqlDB = new ProductTaxesEntities())
            {
                //using (var db = new ProductTaxesEntities())
                //{
                //    foreach (var item in db.ProductTaxes)
                //    {
                //        Console.WriteLine("{0} {1}", item.ProductName, item.Tax);
                //    }
                //}

                var mongoClient = new MongoClient("mongodb://localhost/");
                var mongoServer = mongoClient.GetServer();
                var productReporstDb = mongoServer.GetDatabase("Supermarket");

                var collection = productReporstDb.GetCollection<JsonReport.ProductReport>("ProductReports");
                var expenses = productReporstDb.GetCollection<JsonReport.VendorReport>("VendorReports");
                var random = new Random();
                HashSet<int> used = new HashSet<int>();

                foreach (var item in collection.FindAll())
                {
                    var oldReport = sqlDB.ProductReports.Where(x => x.ProductID == item.ProductId).FirstOrDefault();

                    if (oldReport == null && !used.Contains(item.ProductId))
                    {
                        used.Add(item.ProductId);
                        sqlDB.ProductReports.Add(new SQLiteModel.ProductReport
                        {
                            ProductReportID = item.ProductId,
                            ProductID = item.ProductId,
                            TotalIncomes = item.TotalIncoms,
                            TotalQuantitySold = decimal.Parse(item.TotalQuantitySold.ToString()),
                            VendorName = item.Vendor,
                            ProductName = item.Name

                        });
                    }


                }
                sqlDB.SaveChanges();
                DateTime month = new DateTime(2013, 08, 1);
                var reports = sqlDB.ProductReports.GroupBy(x => x.VendorName);
                string monthTest = month.ToString("MMM-yyyy");
                foreach (var item in reports)
                {
                    Console.WriteLine("Vendor: {0}", item.Key);
                    var income =  item.Sum(x => x.TotalIncomes);
                    Console.WriteLine("Income: {0}",income);
                    var currentExpense = expenses.FindAll().Where(x => x.Vendor == item.Key && x.Month == monthTest).First().Expenses;
                    Console.WriteLine("Expenses: {0}", currentExpense);
                    //var taxes;

                    
                    var products = item.GroupBy(x => x.ProductID);
                    decimal? taxes = 0;
                    decimal? totalIncome = 0;
                    foreach (var pro in products)
                    {
                        totalIncome += pro.Sum(x => x.TotalIncomes);
                        var another = pro.First().ProductID;
                        var productID = sqlDB.ProductTaxes.Where(x=> x.ID == another).First();
                        taxes += (totalIncome*productID.Tax)/100m;
                    }

                    Console.WriteLine("Taxes: {0}", taxes);
                    


                }

                //foreach (var item2 in expenses.FindAll())
                //{
                //    Console.WriteLine(item2.Month == monthTest);
                //}
            }
        }
    }
}
