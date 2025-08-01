using BAL.Service;
using DAL.Entities;
using System.Windows;
using System.Windows.Controls;

namespace BlindBoxShopManagement
{
    public partial class AddStaffWindow : Window
    {
        private readonly AccountService _accountService;
        
        public AddStaffWindow()
        {
            _accountService = new AccountService();
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MessageBox.Show("Password is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Email is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Full name is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    MessageBox.Show("Phone is required.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtIdentity.Text))
                {
                    MessageBox.Show("Identity number is required.");
                    return;
                }

                if (cbGender.SelectedItem == null)
                {
                    MessageBox.Show("Gender must be selected.");
                    return;
                }

                if (!dpDOB.SelectedDate.HasValue)
                {
                    MessageBox.Show("Date of Birth is required.");
                    return;
                }

                // Check age >= 18
                DateTime dob = dpDOB.SelectedDate.Value;
                int age = DateTime.Today.Year - dob.Year;
                if (dob.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 18)
                {
                    MessageBox.Show("Staff must be at least 18 years old.");
                    return;
                }

                // Create and save
                var account = new Account
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Password,
                    Role = "Staff",
                    Email = txtEmail.Text.Trim(),
                };

                var detail = new AccountDetail
                {
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Gender = ((ComboBoxItem)cbGender.SelectedItem).Content.ToString(),
                    DateOfBirth = DateOnly.FromDateTime(dob),
                    IdentityNumber = txtIdentity.Text.Trim(),
                    Avatar = txtAvatar.Text.Trim(),
                };

                _accountService.AddStaffAccount(account, detail);
                MessageBox.Show("Staff account created successfully.");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show("Error: " + error);
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
