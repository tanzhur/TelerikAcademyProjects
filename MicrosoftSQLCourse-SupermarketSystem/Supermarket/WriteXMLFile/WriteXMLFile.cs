using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Xml.Linq;
using Catalog.Data;
using Catalog.Model;
using MySQLCatalog.Model;

namespace WriteXMLFile
{
    public class WriteXMLFile
    {
        static void Main(string[] args)
        {
            string path = @"../../TEST.xml";
            CreateXMLFile(path);
        }

        public static void CreateXMLFile(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter document = XmlWriter.Create(path);

            document.WriteStartDocument();
            document.WriteStartElement("Sales");

            using (var db = new CatalogContext())
            {
                var vendorReport = db.Sales
                    .Include("Product")
                    .Include("Product.Vendor")
                    .GroupBy(x => new { x.Date, x.Product.Vendor.Name })
                    .OrderBy(x => x.Key.Name);

                string vendor = "";
                foreach (var item in vendorReport)
                {
                    if (vendor != item.Key.Name)
                    {
                        if (vendor != string.Empty)
                        {
                            document.WriteEndElement();
                        }
                        vendor = item.Key.Name;
                        document.WriteStartElement("sale");
                        document.WriteAttributeString("vendor", vendor);
                    }

                    document.WriteStartElement("summary");
                    document.WriteAttributeString("date", item.Key.Date.ToString("dd-MMM-yyyy"));
                    document.WriteAttributeString("total-sum", item.Sum(x => x.Sum).ToString());
                    document.WriteEndElement();
                }

                document.WriteEndDocument();
            }

            document.Close();
        }
    }
}


