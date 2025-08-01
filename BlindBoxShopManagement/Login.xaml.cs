using BAL.Service;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly AccountService accountService;

        public Login()
        {
            InitializeComponent();
            accountService = new AccountService();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            String email = accountTxt.Text.Trim();
            String password = txtPassword.Password.Trim();
            
            if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(accountService.ValidateCredentials(email, password))
            {
                MessageBox.Show("Login successful!");
                String role = accountService.GetRoleByEmail(email);
                ShopManagementWindow window = new ShopManagementWindow(email);
                window.Show();
                this.Close(); // Close the login window
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
