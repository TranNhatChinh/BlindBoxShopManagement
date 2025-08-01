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
        public ShopManagementWindow(string role)
        {
            InitializeComponent();
            this.role = role;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


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

        private void Button_Order(object sender, RoutedEventArgs e)
        {

        }

        private void ProductManagementClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
