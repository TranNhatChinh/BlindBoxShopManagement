using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Service
{
    public class AccountService
    {
        private readonly AccountRepository accountRepository;
        public AccountService()
        {
            accountRepository = new AccountRepository();
        }

        public bool ValidateCredentials(string email, string password)
        {
            var account = accountRepository.GetAccountByEmail(email);
            if (account == null)
            {
                return false; // Account not found
            }
            else if (!account.Password.Equals(password))
            {
                return false; // Password does not match
            }
            else
            {
                return true; // Credentials are valid
            }
        }

        public List<AccountDetail> getAllStaffDetails()
        {
            return accountRepository.getAllStaffDetails();
        }

        public string GetRoleByEmail(string email)
        {
            return accountRepository.GetRoleByEmail(email);
        }

        public void AddStaffAccount(Account account, AccountDetail detail)
        {
            if (account == null || detail == null)
            {
                throw new ArgumentNullException("Account or AccountDetail cannot be null");
            }
            accountRepository.AddStaffAccount(account, detail);
        }

        public void UpdateStaffAccount(Account account, AccountDetail detail)
        {
            if (account == null || detail == null)
            {
                throw new ArgumentNullException("Account or AccountDetail cannot be null");
            }
            // Assuming the repository has an Update method
            accountRepository.UpdateStaffAccount(account, detail);
        }

        public void deleteStaffAccount(Account account)
        {
            accountRepository.DeleteStaffAccount(account);
        }
    }
}
