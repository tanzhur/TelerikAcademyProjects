using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data.Sql;
using Catalog.Model;
using Catalog.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySQLCatalog.Model;
using TestConnections;

namespace PDFGenerator
{
    public partial class PDFGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var document = new Document();
            string path = Server.MapPath("PDFs");
            PdfWriter.GetInstance(document, new FileStream(path + "/document.pdf", FileMode.Create));
            document.Open();
            PdfPTable table = new PdfPTable(4);

            ////actual width of table in points
            //table.TotalWidth = 216f;

            ////fix the absolute width of the table
            //table.LockedWidth = true;

            ////relative col widths in proportions - 1/3 and 2/3
            //float[] widths = new float[] { 1f, 2f };
            //table.SetWidths(widths);
            //table.HorizontalAlignment = 0;

            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //PdfPCell cell = new PdfPCell(new Phrase("Products"));

            //cell.Colspan = 2;
            //cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            //table.AddCell(cell);
        //    string connect = "Server = NNAIDENOV-PC;Database=supermarket; Integrated Security=true";
            string connect = "server=192.168.194.189;database=SalesReports;uid=coffeeUser;pwd=coffee123";
           // new SqlConnection("Server = Xri-PC;Database=northwind; Integrated Security=true");


            using (SqlConnection conn = new SqlConnection(connect))
            {
                string query = "SELECT * FROM Products";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            table.AddCell(rdr[0].ToString());
                            table.AddCell(rdr[1].ToString());
                            table.AddCell(rdr[2].ToString());
                            table.AddCell(rdr[3].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                document.Add(table);
                document.Close();
            }
        }
    }
}

 