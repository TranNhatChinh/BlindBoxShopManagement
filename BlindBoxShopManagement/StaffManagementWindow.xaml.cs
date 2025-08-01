using BAL.Service;
using DAL.Entities;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using BlindBoxShopManagement.Utils;


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
                    ExportUtils.ExportToCsv(path, staffList);
                }
                else if (extension == ".xlsx")
                {
                    ExportUtils.ExportToExcel(path, staffList);
                }

                MessageBox.Show("Export completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export error: " + ex.Message);
            }
        }
    }
}
