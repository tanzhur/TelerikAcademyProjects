using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using Catalog.Data;
using Catalog.Model;
using Ionic.Zip;
using System.IO;
using System.Globalization;

namespace OpenZipFile
{
    public class OpenZipFileClient
    {
        private static readonly List<string> fileNames = new List<string>();
        private static readonly List<string> directoryNames = new List<string>();
        private static readonly Dictionary<string, List<string>> paths = new Dictionary<string, List<string>>();

        static void Main(string[] args)
        {
            string zipFileName = @"../../File.zip";
            string outputDirectory = "../../Extracted";


            OpenZipFile(zipFileName, outputDirectory);

            GetExcelData();
            DeleteExtractedFiles(outputDirectory);
        }

        public static void DeleteExtractedFiles(string path)
        {
            Directory.Delete(path, true);
        }

        public static void OpenZipFile(string zipFileName, string outputDirectory)
        {
            using (ZipFile zip = ZipFile.Read(zipFileName))
            {
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
                    if (entry.IsDirectory)
                    {
                        directoryNames.Add(entry.FileName);
                    }
                    else
                    {
                        string[] arr = new string[2];
                        arr = entry.FileName.Split('/');
                        if (paths.ContainsKey(arr[0]))
                        {
                            paths[arr[0]].Add(arr[1]);
                        }
                        else
                        {
                            paths.Add(arr[0], new List<string> { arr[1] });
                        }

                        fileNames.Add(arr[1]);
                    }
                }
            }
        }

        public static void GetExcelData()
        {
            using (var db = new CatalogContext())
            {
                HashSet<string> addedShops = new HashSet<string>();

                //this is used for avoiding duplicates and fast search
                foreach (var item in db.Shops)
                {
                    addedShops.Add(item.Name);
                }

                foreach (var path in paths)
                {
                    foreach (var fileName in path.Value)
                    {
                        AddShop(fileName, addedShops, db);
                    }
                }

                db.SaveChanges();
              AddSale(db, paths);
                db.SaveChanges();
            }
        }

        private static void AddSale(CatalogContext db, Dictionary<string, List<string>> paths)
        {
            foreach (var path in paths)
            {
                foreach (var fileName in path.Value)
                {
                    // used for gathering and combing the sells of the same products and same shop
                    Dictionary<int, Sale> currentSales = new Dictionary<int, Sale>();

                    OleDbConnection excelConnection = 
                        new OleDbConnection(
                           "Provider=Microsoft.ACE.OLEDB.12.0;" +
                          @"Data Source=..\..\Extracted\" + path.Key + "\\" + fileName + ";" +
                          @"Extended Properties=""Excel 8.0;HDR=YES""");

                    string shopName = fileName.Split((new string[] { "-Sales" }), StringSplitOptions.None).First();

                    excelConnection.Open();
                    using (excelConnection)
                    {
                        
                        
                        using   (
                                OleDbCommand command = 
                                new OleDbCommand(
                                String.Format("SELECT * FROM [{0}${1}]", "Sales", "B3:E"), excelConnection)
                                )
                        {
                            
                            command.Connection = excelConnection;

                            OleDbDataReader dataExcel = command.ExecuteReader();
                            using (dataExcel)
                            {
                                while (dataExcel.Read())
                                {
                                    var productId = dataExcel["ProductID"];
                                    int productIdFull = 0;

                                    if (productId != DBNull.Value)
                                    {
                                        productIdFull = Convert.ToInt32(productId);
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    //Console.WriteLine("{0}", productIdFull);
                                    DateTime date = DateTime.Parse(path.Key);
                                    //Console.WriteLine("{0}", date);
                                    decimal sum = Convert.ToDecimal(dataExcel["Sum"]);
                                    //Console.WriteLine("{0}", sum);
                                    decimal quantity = Convert.ToDecimal(dataExcel["Quantity"]);
                                    //Console.WriteLine("{0}", quantity);
                                    decimal unitPrice = Convert.ToDecimal(dataExcel["Unit Price"]);
                                    //Console.WriteLine("{0}", unitPrice);

                                    int shopID = db.Shops.Where(x => x.Name.CompareTo(shopName) == 0).First().ShopID;

                                    if (currentSales.ContainsKey(productIdFull))
                                    {
                                        currentSales[productIdFull].Quantity += quantity;
                                        currentSales[productIdFull].Sum += sum;
                                    }
                                    else
                                    {
                                        Sale newSale = new Sale
                                        {
                                            Date = date,
                                            ProductID = productIdFull,
                                            Quantity = quantity,
                                            UnitPrice = unitPrice,
                                            Sum = sum,
                                            ShopID = shopID
                                        };

                                        currentSales[productIdFull] = newSale;
                                    }
                                }
                            }
                        }
                    }
                    
                    foreach (var item in currentSales)
                    {
                        db.Sales.Add(item.Value);
                    }
                }
            }
        }


        public static void AddShop(string fileName, HashSet<string> addedShops, CatalogContext db)
        {
            var myShop = new Shop();
            string shopName = fileName.Split((new string[] { "-Sales" }), StringSplitOptions.None).First();
            myShop.Name = shopName;

            if (!addedShops.Contains(shopName))
            {
                db.Shops.Add(myShop);
            }
        }
    }
}