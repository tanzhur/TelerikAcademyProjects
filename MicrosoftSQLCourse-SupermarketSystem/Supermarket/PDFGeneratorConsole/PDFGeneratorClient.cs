using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Catalog.Data;

namespace PDFGeneratorConsole
{
 public   class PDFGeneratorClient
    {
        static void Main(string[] args)
        {
            CreatePDFDocument();
        }

        public static void CreatePDFDocument()
        {
            var document = new Document();
            //string path = Server.MapPath("PDFs");
            PdfWriter.GetInstance(document, new FileStream("../../document.pdf", FileMode.Create));
            document.Open();
            PdfPTable table = new PdfPTable(5);


            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            using (var db = new CatalogContext())
            {

                //one query to the database
                var sales = db.Sales
                    .Include("Product")
                    .Include("Shop")
                    .OrderBy(x => new { x.Date, x.ProductID, x.ShopID })
                    .ToList();
                DateTime reportDate = new DateTime();
                decimal totalSum = 0;
                foreach (var sale in sales)
                {
                    if (reportDate != sale.Date)
                    {
                        if (totalSum != 0)
                        {
                            table.AddCell("");
                            table.AddCell("");
                            table.AddCell("");
                            table.AddCell(string.Format("Total sum for: {0}", reportDate.ToString("dd-MMM-yyyy")));
                            //PdfPCell total = new PdfPCell(new Phrase(string.Format("Total: {0}", totalSum)));
                            //total.HorizontalAlignment = 1;
                            //total.Colspan = 4;

                            //table.AddCell(total);
                            table.AddCell(string.Format("Total: {0}", totalSum));
                            totalSum = 0;
                        }
                        table.AddCell(string.Format("Date: {0}", sale.Date.ToString("dd-MMM-yyyy")));
                        table.CompleteRow();
                        table.AddCell("Product");
                        table.AddCell("Quantity");
                        table.AddCell("UnitPrice");
                        table.AddCell("Location");
                        table.AddCell("Sum");
                        reportDate = sale.Date;
                    }
                    totalSum += sale.Sum;
                    table.AddCell(sale.Product.ProductName);
                    table.AddCell(sale.Quantity.ToString());
                    table.AddCell(sale.UnitPrice.ToString());
                    table.AddCell(sale.Shop.Name);
                    table.AddCell(sale.Sum.ToString());
                }

                document.Add(table);
                document.Close();
            }
        }
    }
}
