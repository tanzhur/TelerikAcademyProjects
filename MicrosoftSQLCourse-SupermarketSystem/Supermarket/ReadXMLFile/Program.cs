using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;
using JsonReport;
using Catalog.Data;
using Catalog.Model;

namespace ReadXMLFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../file.xml";
            Dictionary<string, Dictionary<string, decimal>> data = new Dictionary<string, Dictionary<string, decimal>>();


            XmlDocument doc = new XmlDocument();
            doc.Load(@"../../file.xml");
            XmlNode rootNode = doc.DocumentElement;
            //Console.WriteLine("Root node: {0}", rootNode.Name);

            decimal expense = 0m;
            string txt = "";
            int count = 0;
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                string vendor = node.Attributes["vendor"].Value.ToString();
                string month = "";
                //Console.WriteLine(vendor);
                foreach (XmlNode child in node)
                {
                    month = child.Attributes["month"].Value.ToString();
                    txt = child.InnerText.ToString();
                    expense = decimal.Parse(txt, CultureInfo.InvariantCulture);

                    if (!data.ContainsKey(vendor))
                    {
                        data[vendor] = new Dictionary<string, decimal>();
                    }
                    data[vendor].Add(month, expense);

                    VendorReport currentReport = new VendorReport(vendor, month, expense);
                    JsonClient.InsertIntoMongoVendorReport(currentReport, count);
                    count++;
                }
            }

            using (var db = new CatalogContext())
            {
                //one query to database
                var currentVendors = db.Vendors.ToList();
                var currentExpenses = db.VendorExpenses.ToList();

                foreach (var vendor in data)
                {

                    int vendorID = currentVendors
                        .Where(x => x.Name.CompareTo(vendor.Key) == 0)
                        .Select(x => x.VendorId).First();

                    foreach (var vendorExpense in vendor.Value)
                    {
                        VendorExpense newExpense = new VendorExpense
                            {
                                VendorID = vendorID,
                                Month = DateTime.Parse(vendorExpense.Key),
                                Expense = vendorExpense.Value
                            };
                        var currentExpense = currentExpenses.Where(x => x.VendorID == vendorID && x.Month == DateTime.Parse(vendorExpense.Key)).FirstOrDefault();
                        if (currentExpense != null)
                        {
                            currentExpense.Expense = vendorExpense.Value;
                        }
                        else
                        {
                            db.VendorExpenses.Add(newExpense);
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
