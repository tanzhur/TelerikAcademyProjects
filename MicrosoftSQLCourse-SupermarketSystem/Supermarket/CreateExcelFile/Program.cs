using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using OfficeOpenXml;
using System.IO;
using System.Media;

namespace CreateExcelFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../ProductTaxes.db";
            string output = @"../../EXCELTABLE.xlsx";
            FileInfo newFile = new FileInfo(output);
            // FileInfo newFile = new FileInfo(ExportFileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            CreateSheet(pck, "TESTSHEET");

        }
        private static ExcelWorksheet CreateSheet(ExcelPackage p, string sheetName)
        {
            int row = 1;
            int col = 1;
            if (p.Workbook.Worksheets.Count == 0)
            {
                p.Workbook.Worksheets.Add("Sheet1");
                // ExcelWorksheet ws = p.Workbook.Worksheets[1];
            }

            // p.Workbook.Worksheets.Add("Sheet1");
            ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ws.Cells[1, 1].Value = "Vendor";
            ws.Cells[1, 2].Value = "Incomes";
            ws.Cells[1, 3].Value = "Expenses";
            ws.Cells[1, 4].Value = "Taxes";
            ws.Cells[1, 5].Value = "Financial Result";
            ws.Cells[1, 1, 1, 5].Style.Font.Bold = true;


            p.Save();
            return ws;

        }
    }
}
