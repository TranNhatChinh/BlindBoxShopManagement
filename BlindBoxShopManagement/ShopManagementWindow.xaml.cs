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
    /// Interaction logic for ShopManagementWindow.xaml
    /// </summary>
    public partial class ShopManagementWindow : Window
    {
        private readonly String role;

        private readonly ProductService productService;
        private readonly AccountService accountService;
        private readonly bool isAdmin;
        private string Admin_ROLE = "Admin"; // Assuming 2 is manager role ID

        public ShopManagementWindow(string userEmail)
        {
            InitializeComponent();
            productService = new ProductService();
            accountService = new AccountService();
            if (accountService.GetRoleByEmail(userEmail).Equals(Admin_ROLE)==true)
                isAdmin = true;
            MessageBox.Show($"Welcome {userEmail}! You are logged in as {(isAdmin ? "Admin" : "Staff")}.");
            // Enable/disable CRUD buttons based on role
            btnAdd.IsEnabled = isAdmin;
            btnUpdate.IsEnabled = isAdmin;
            btnDelete.IsEnabled = isAdmin;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProduct();
        }
        private void LoadProduct()
        {
            dgvDisplay.ItemsSource = productService.GetAllProducts();
        }
        private void dgvDisplay_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedInventory = dgvDisplay.SelectedItem as Product;
            if (selectedInventory == null)
            {
                // Clear all UI fields
                txtId.Text = string.Empty;
                txtName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtStock.Text = string.Empty;
                dpCreatedAt.SelectedDate = null;
                return;
            }

            // Update text fields
            txtId.Text = selectedInventory.Id.ToString();
            txtName.Text = selectedInventory.Name.ToString();
            txtPrice.Text = selectedInventory.Price.ToString("F2");
            dpCreatedAt.SelectedDate = selectedInventory.CreatedAt.Date;
            txtDescription.Text = selectedInventory.Description.ToString();
            txtStock.Text = selectedInventory.Stock.ToString();

        }

        private void btnSearchByProductName_Click(object sender, RoutedEventArgs e)
        {
            
            var supplier = txtSearchByProductName.Text.Trim();
            if (string.IsNullOrEmpty(supplier))
            {
                LoadProduct();
                return;
            }
            else
            {
                dgvDisplay.ItemsSource = productService.GetAllProductsByProductName(supplier);
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedInventory = dgvDisplay.SelectedItem as Product;
            if (selectedInventory != null)
            {
                if (MessageBox.Show("Do you want to delete this inventory?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    productService.DeleteProduct(selectedInventory.Id);
                    LoadProduct();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please select an inventory to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public Product getInputInventory()
        {
            try
            {
                var Id = txtId.Text.Trim();
                var Name = txtName.Text.Trim();
                var Description = txtDescription.Text.Trim();
                var price = txtPrice.Text.Trim();
                var CreatedAt = dpCreatedAt.SelectedDate;
                var Stock = txtStock.Text.Trim();


                Product a = new Product()
                {
                    Id = int.TryParse(Id, out int invId) ? invId : 0,
                    Name = Name,
                    Description = Description,
                    Price = decimal.TryParse(price, out decimal prc) ? prc : 0,
                    CreatedAt = CreatedAt ?? DateTime.Now,
                    Stock = int.TryParse(Stock, out int stock) ? stock : 0
                };
                return a;

            }
            catch
            {
                return null;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            

            Product a = getInputInventory();
            if (a != null)
            {
                
                productService.AddProduct(a);
                LoadProduct();
            }
            return;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            

            Product a = getInputInventory();
            if (a != null)
            {
                var selectedInventory = dgvDisplay.SelectedItem as Product;
                selectedInventory.Name = a.Name;
                selectedInventory.Description = a.Description;
                selectedInventory.Price = a.Price;
                selectedInventory.CreatedAt = a.CreatedAt;
                selectedInventory.Stock = a.Stock;
                if (selectedInventory != null)
                {
                    productService.UpdateProduct(selectedInventory);
                    LoadProduct();
                    MessageBox.Show("Product updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a product to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return;
        }

    }
}
