using DAL.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Text;

namespace BlindBoxShopManagement.Utils
{
    public static class ExportUtils
    {
        public static void ExportToCsv(string filePath, List<AccountDetail> staffList)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Full Name,Phone,Address,Username,Email,Role");

            foreach (var s in staffList)
            {
                csv.AppendLine(string.Join(",", new[]
                {
                    EscapeCsv(s.FullName),
                    EscapeCsv(s.Phone),
                    EscapeCsv(s.Address),
                    EscapeCsv(s.Account?.Username),
                    EscapeCsv(s.Account?.Email),
                    EscapeCsv(s.Account?.Role)
                }));
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        public static void ExportToExcel(string filePath, List<AccountDetail> staffList)
        {
            ExcelPackage.License.SetNonCommercialPersonal("PRN212_Project");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Staff List");

            worksheet.Cells[1, 1].Value = "Full Name";
            worksheet.Cells[1, 2].Value = "Phone";
            worksheet.Cells[1, 3].Value = "Address";
            worksheet.Cells[1, 4].Value = "Username";
            worksheet.Cells[1, 5].Value = "Email";
            worksheet.Cells[1, 6].Value = "Role";

            using (var header = worksheet.Cells[1, 1, 1, 6])
            {
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            for (int i = 0; i < staffList.Count; i++)
            {
                var s = staffList[i];
                worksheet.Cells[i + 2, 1].Value = s.FullName;
                worksheet.Cells[i + 2, 2].Value = s.Phone;
                worksheet.Cells[i + 2, 3].Value = s.Address;
                worksheet.Cells[i + 2, 4].Value = s.Account?.Username;
                worksheet.Cells[i + 2, 5].Value = s.Account?.Email;
                worksheet.Cells[i + 2, 6].Value = s.Account?.Role;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            package.SaveAs(new FileInfo(filePath));
        }

        public static void ExportOrdersToCsv(string filePath, List<Order> orders)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Order ID,Customer Name,Created At,Total Price");

            foreach (var o in orders)
            {
                csv.AppendLine(string.Join(",", new[]
                {
                    o.Id.ToString(),
                    EscapeCsv(o.CustomerName ?? ""),
                    o.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                    o.TotalPrice?.ToString("0.00")
                }));
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
        }

        public static void ExportOrdersToExcel(string filePath, List<Order> orders)
        {
            ExcelPackage.License.SetNonCommercialPersonal("PRN212_Project");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Orders");

            worksheet.Cells[1, 1].Value = "Order ID";
            worksheet.Cells[1, 2].Value = "Customer Name";
            worksheet.Cells[1, 3].Value = "Created At";
            worksheet.Cells[1, 4].Value = "Total Price";

            using (var header = worksheet.Cells[1, 1, 1, 4])
            {
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            for (int i = 0; i < orders.Count; i++)
            {
                var o = orders[i];
                worksheet.Cells[i + 2, 1].Value = o.Id;
                worksheet.Cells[i + 2, 2].Value = o.CustomerName ?? "";
                worksheet.Cells[i + 2, 3].Value = o.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cells[i + 2, 4].Value = o.TotalPrice;
                worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "#,##0";
            }

            int totalRow = orders.Count + 2;
            worksheet.Cells[totalRow, 3].Value = "Total Income:";
            worksheet.Cells[totalRow, 4].Formula = $"SUM(D2:E{orders.Count + 1})";
            worksheet.Cells[totalRow, 4].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[totalRow, 3, totalRow, 4].Style.Font.Bold = true;

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            package.SaveAs(new FileInfo(filePath));
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";
            value = value.Replace("\"", "\"\"");
            return value.Contains(',') || value.Contains('"') || value.Contains('\n') ? $"\"{value}\"" : value;
        }
    }
}
