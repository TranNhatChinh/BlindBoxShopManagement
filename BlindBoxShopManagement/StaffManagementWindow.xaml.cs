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
        public StaffManagementWindow()
        {
            InitializeComponent();
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
            String searchText = txtSearch.Text.Trim();
            if (searchText.Equals("Search by Full Name") || string.IsNullOrWhiteSpace(searchText))
            {
                //Show all staff
                MessageBox.Show("Please enter a valid name to search.");
                return;
            }
            else
            {
                // Perform search logic here
            }
        }
    }
}
