using BAL.Service;
using DAL.Entities;
using System.Windows;
using System.Windows.Controls;

namespace BlindBoxShopManagement
{
    public partial class AddStaffWindow : Window
    {
        private readonly AccountService _accountService;
        private Account? _editingAccount;
        private AccountDetail? _editingDetail;
        public AddStaffWindow(Account account, AccountDetail detail)
        {
            InitializeComponent();
            _accountService = new AccountService();

            // Populate fields with existing data
            txtUsername.Text = account.Username;
            txtEmail.Text = account.Email;
            txtPassword.Password = account.Password;

            txtFullName.Text = detail.FullName;
            txtPhone.Text = detail.Phone;
            txtAddress.Text = detail.Address;
            cbGender.SelectedItem = cbGender.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(i => i.Content.ToString() == detail.Gender);
            dpDOB.SelectedDate = detail.DateOfBirth?.ToDateTime(new TimeOnly());
            txtIdentity.Text = detail.IdentityNumber;
            txtAvatar.Text = detail.Avatar;

            // Store for later use
            _editingAccount = account;
            _editingDetail = detail;
        }

        public AddStaffWindow()
        {
            _accountService = new AccountService();
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs(out string errorMessage))
                {
                    MessageBox.Show(errorMessage);
                    return;
                }

                // Extracting values
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Password;
                string email = txtEmail.Text.Trim();
                string fullName = txtFullName.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string address = txtAddress.Text.Trim();
                string gender = ((ComboBoxItem)cbGender.SelectedItem).Content.ToString();
                DateTime dob = dpDOB.SelectedDate.Value;
                string identity = txtIdentity.Text.Trim();
                string avatar = txtAvatar.Text.Trim();

                if (_editingAccount != null && _editingDetail != null)
                {
                    // Edit mode
                    _editingAccount.Username = txtUsername.Text.Trim();
                    _editingAccount.Email = txtEmail.Text.Trim();
                    _editingAccount.Password = txtPassword.Password;

                    _editingDetail.FullName = txtFullName.Text.Trim();
                    _editingDetail.Phone = txtPhone.Text.Trim();
                    _editingDetail.Address = txtAddress.Text.Trim();
                    _editingDetail.Gender = ((ComboBoxItem)cbGender.SelectedItem)?.Content.ToString();
                    _editingDetail.DateOfBirth = dpDOB.SelectedDate.HasValue ? DateOnly.FromDateTime(dpDOB.SelectedDate.Value) : null;
                    _editingDetail.IdentityNumber = txtIdentity.Text.Trim();
                    _editingDetail.Avatar = txtAvatar.Text.Trim();

                    _accountService.UpdateStaffAccount(_editingAccount, _editingDetail);
                }
                else
                {
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
                }
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

        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorMessage = "Username is required.";
            }
            else if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                errorMessage = "Password is required.";
            }
            else if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorMessage = "Email is required.";
            }
            else if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                errorMessage = "Full name is required.";
            }
            else if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorMessage = "Phone is required.";
            }
            else if (string.IsNullOrWhiteSpace(txtIdentity.Text))
            {
                errorMessage = "Identity number is required.";
            }
            else if (cbGender.SelectedItem == null)
            {
                errorMessage = "Gender must be selected.";
            }
            else if (!dpDOB.SelectedDate.HasValue)
            {
                errorMessage = "Date of Birth is required.";
            }
            else
            {
                // Check age >= 18
                DateTime dob = dpDOB.SelectedDate.Value;
                int age = DateTime.Today.Year - dob.Year;
                if (dob.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 18)
                {
                    errorMessage = "Staff must be at least 18 years old.";
                }
            }

            return string.IsNullOrEmpty(errorMessage);
        }

    }
}
