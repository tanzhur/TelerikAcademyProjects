using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catalog.Data;
using System.Data.Entity;
using Catalog.Data.Migrations;
using System.Data.OleDb;
using OpenZipFile;

namespace TestConnections
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new Configuration());

            using (var db = new CatalogContext())
            {
                var sale = db.Vendors.First();
                //var vendorReport = db.Sales
                //    .Include("Product")
                //    .Include("Product.Vendor")
                //    .GroupBy(x => new {  x.Date, x.Product.Vendor.Name })
                //    .OrderBy(x=> x.Key.Name);

                //foreach (var vendor in vendorReport)
                //{
                //    Console.WriteLine("Vendor: {0}", vendor.Key.Name);
                //    //Console.WriteLine("ProductId: {0}", vendor.Key.ProductID);
                //    Console.WriteLine("Sum: {0}", vendor.Sum(x=> x.Sum));
                //}

                //Console.WriteLine("test");
                //var productReport = db.Sales
                //    //.Include("Product")
                //    //.Include("Product.Vendor")
                //    .Select(x => new { 
                //                        x.Sum,
                //                        x.Quantity, 
                //                        x.ProductID, 
                //                        x.Product.ProductName, 
                //                        x.Product.Vendor.Name
                //                     }
                //            )
                //    .GroupBy(x => x.ProductID);
               
                //foreach (var item in productReport)
                //{
                //    Console.WriteLine("ID: {0} Name: {1} Vendor: {2} Quantity: {3} Sum: {4}",
                //        item.Key,
                //        item.First().ProductName,
                //        item.First().Name,
                //        item.Sum(x => x.Quantity),
                //        item.Sum(x => x.Sum)
                //        );
                //}

                //Console.WriteLine("test");

                //Console.WriteLine("{0,15} {1,15} {2,15} {3,15} {4,15}", "Product", "Quantity", "UnitPrice", "Location", "Sum");
               
                //var sales = db.Sales
                //    .Include("Product")
                //    .Include("Shop")
                //    .OrderBy(x => new { x.Date, x.ProductID, x.ShopID})
                //    .ToList();
                //DateTime reportDate = new DateTime();
                
                //foreach (var sale in sales)
                //{
                //    if (reportDate != sale.Date)
                //    {
                //        Console.WriteLine("Date: {0}", sale.Date);
                //        reportDate = sale.Date;
                //    }

                //    Console.WriteLine("{0,15} {1,15} {2,15} {3,15} {4,10:F2}",
                //       sale.Product.ProductName,
                //       sale.Quantity,
                //       sale.UnitPrice,
                //       sale.Shop.Name,
                //       sale.Sum
                //       );
                //}
            }
        }
    }
}
