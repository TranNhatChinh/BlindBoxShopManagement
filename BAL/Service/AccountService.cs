using DAL;
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

        public string GetRoleByEmail(string email)
        {
            return accountRepository.GetRoleByEmail(email);
        }
    }
}
