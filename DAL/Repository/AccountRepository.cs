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



        public string GetRoleByEmail(string email)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);
            return account?.Role ?? "User"; // Default to "User" if not found
        }
    }
}
