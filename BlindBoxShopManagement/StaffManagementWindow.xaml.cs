using BAL.Service;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlindBoxShopManagement
{
    /// <summary>
    /// Interaction logic for StaffManagementWindow.xaml
    /// </summary>
    public partial class StaffManagementWindow : Window
    {

        private AccountService _accountService;
        private readonly string role;

        public StaffManagementWindow(string role)
        {
            InitializeComponent();
            _accountService = new AccountService();
            this.role = role;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load staff data into the DataGrid
            LoadStaffData();
            if (!role.Equals("Admin"))
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
            var addWindow = new AddStaffWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                LoadStaffData();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            var staffList = _accountService.getAllStaffDetails();

            if (string.IsNullOrWhiteSpace(searchText) || searchText.Equals("Search by Full Name", StringComparison.OrdinalIgnoreCase))
            {
                // Show all staff
                dgvDisplay.ItemsSource = staffList;
            }
            else
            {
                // Filter by full name (case-insensitive)
                var filtered = staffList
                    .Where(s => s.FullName != null && s.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                dgvDisplay.ItemsSource = filtered;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgvDisplay.SelectedItem as AccountDetail;
            if (selected == null)
            {
                MessageBox.Show("Please select a staff to edit.");
                return;
            }

            var account = selected.Account;
            if (account == null)
            {
                MessageBox.Show("Account data is missing.");
                return;
            }

            var editWindow = new AddStaffWindow(account, selected);
            var result = editWindow.ShowDialog();
            if (result == true)
            {
                // Refresh after edit
                dgvDisplay.ItemsSource = _accountService.getAllStaffDetails();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgvDisplay.SelectedItem as AccountDetail;
            if (selected == null)
            {
                MessageBox.Show("Please select a staff to delete.");
                return;
            }

            var account = selected.Account;
            if (account == null)
            {
                MessageBox.Show("Account data is missing.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete {selected.FullName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    _accountService.deleteStaffAccount(account);
                    LoadStaffData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
