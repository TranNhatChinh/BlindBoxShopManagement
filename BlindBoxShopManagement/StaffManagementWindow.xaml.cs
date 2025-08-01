using BAL.Service;
using DAL.Entities;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace BlindBoxShopManagement
{
    public partial class StaffManagementWindow : Window
    {
        private readonly AccountService _accountService;
        private readonly string role;

        public StaffManagementWindow(string role)
        {
            InitializeComponent();
            _accountService = new AccountService();
            this.role = role;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStaffData();

            if (!string.Equals(role?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase))
            {
                addBtn.IsEnabled = false;
                editBtn.IsEnabled = false;
                deleteBtn.IsEnabled = false;
            }
        }

        private void LoadStaffData()
        {
            dgvDisplay.ItemsSource = _accountService.getAllStaffDetails();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search by Full Name")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search by Full Name";
                txtSearch.Foreground = Brushes.Gray;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddStaffWindow { Owner = this };
            if (addWindow.ShowDialog() == true)
            {
                LoadStaffData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgvDisplay.SelectedItem is not AccountDetail selected)
            {
                MessageBox.Show("Please select a staff to edit.");
                return;
            }

            if (selected.Account == null)
            {
                MessageBox.Show("Account data is missing.");
                return;
            }

            var editWindow = new AddStaffWindow(selected.Account, selected) { Owner = this };
            if (editWindow.ShowDialog() == true)
            {
                LoadStaffData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvDisplay.SelectedItem is not AccountDetail selected)
            {
                MessageBox.Show("Please select a staff to delete.");
                return;
            }

            if (selected.Account == null)
            {
                MessageBox.Show("Account data is missing.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete {selected.FullName}?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    _accountService.deleteStaffAccount(selected.Account);
                    LoadStaffData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            var staffList = _accountService.getAllStaffDetails();

            dgvDisplay.ItemsSource = string.IsNullOrWhiteSpace(searchText) ||
                                     searchText.Equals("Search by Full Name", StringComparison.OrdinalIgnoreCase)
                ? staffList
                : staffList.Where(s => s.FullName != null && s.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private void btnExportStaff_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Export Staff",
                Filter = "Excel Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv",
                FileName = "staff_export"
            };

            if (saveFileDialog.ShowDialog() != true) return;

            var path = saveFileDialog.FileName;
            var extension = Path.GetExtension(path).ToLowerInvariant();
            var staffList = _accountService.getAllStaffDetails();

            try
            {
                if (extension == ".csv")
                {
                    ExportToCsv(path, staffList);
                }
                else if (extension == ".xlsx")
                {
                    ExportToExcel(path, staffList);
                }

                MessageBox.Show("Export completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export error: " + ex.Message);
            }
        }

        private void ExportToCsv(string filePath, List<AccountDetail> staffList)
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

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";
            value = value.Replace("\"", "\"\"");
            return value.Contains(',') || value.Contains('"') || value.Contains('\n') ? $"\"{value}\"" : value;
        }

        private void ExportToExcel(string filePath, List<AccountDetail> staffList)
        {
            ExcelPackage.License.SetNonCommercialPersonal("PRN212_Pro");

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
                header.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
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
    }
}
