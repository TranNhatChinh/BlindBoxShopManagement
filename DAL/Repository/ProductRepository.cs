using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ProductRepository
    {
        private readonly BlindBoxShopContext _context;

        public ProductRepository()
        {
            _context = new BlindBoxShopContext();
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public List<Product> findByProductName(string productName)
        {
            return _context.Products.Where(p => p.Name.Contains(productName)).ToList();
        }

    }
}
