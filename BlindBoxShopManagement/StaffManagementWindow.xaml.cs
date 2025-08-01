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
        public StaffManagementWindow()
        {
            InitializeComponent();
            _accountService = new AccountService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load staff data into the DataGrid
            LoadStaffData();
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
    }
}
