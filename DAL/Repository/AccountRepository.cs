using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class AccountRepository
    {
        private readonly BlindBoxShopContext _context;
        public AccountRepository()
        {
            _context = new BlindBoxShopContext();
        }
        public Account GetAccountByEmail(string email)
        {
            return _context.Accounts.FirstOrDefault(a => a.Email == email);
        }

        public List<AccountDetail> getAllStaffDetails()
        {
            return _context.AccountDetails.Include(ad => ad.Account)
                                          .Where(ad => ad.Account.Role == "Staff")
                                          .ToList();
        }

        public void AddStaffAccount(Account account, AccountDetail detail)
        {
            if (account == null || detail == null)
            {
                throw new ArgumentNullException("Account or AccountDetail cannot be null");
            }
            _context.Accounts.Add(account);
            _context.SaveChanges(); // Ensure account ID is generated

            detail.AccountId = account.Id;
            _context.AccountDetails.Add(detail);
            _context.SaveChanges();
        }

        public void UpdateStaffAccount(Account account, AccountDetail detail)
        {
            _context.Accounts.Update(account);
            _context.AccountDetails.Update(detail);
            _context.SaveChanges();
        }

        public string GetRoleByEmail(string email)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);
            return account?.Role ?? "User"; // Default to "User" if not found
        }

        public void DeleteStaffAccount(Account account)
        {
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }
    }
}
