using BAL.Service;
using System;
using System.Collections;
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
    /// Interaction logic for ShopManagementWindow.xaml
    /// </summary>
    public partial class ShopManagementWindow : Window
    {
        private readonly String role;
        private readonly ProductService productService;
        public ShopManagementWindow(string role)
        {
            InitializeComponent();
            this.role = role;
            productService = new ProductService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load();

            if (!role.Equals("Admin") && !role.Equals("Manager"))
            {
                staffManagementButton.IsEnabled = false;
                ProductManagementButton.IsEnabled = false;
            }
        }

        private void StaffManageClick(object sender, RoutedEventArgs e)
        {
            StaffManagementWindow staffManagementWindow = new StaffManagementWindow();
            staffManagementWindow.Show();

        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close(); // Close the current window
        }

        public void load()
        {
            dgvDisplay.ItemsSource = productService.getAllProducts() ;

        }

        private void Button_Order(object sender, RoutedEventArgs e)
        {

        }

        private void ProductManagementClick(object sender, RoutedEventArgs e)
        {

        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var searchText = txtSearchByProductName.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                dgvDisplay.ItemsSource = productService.getAllProducts();
                return;
            }
            var results =  productService.findByProductName(searchText);
            if (results.Count == 0)
            {
                MessageBox.Show("No product found with the given Pre-Order No.");
            }
            else
            {
                dgvDisplay.ItemsSource = results;
            }
        }
    }
}
