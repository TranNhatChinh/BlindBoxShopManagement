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

        public List<Product> GetAllProduct()
        {
            return _context.Products.ToList();
        }

        public List<Product> GetAllProductsByProductName(string name)
        {
            return _context.Products.Where(x => x.Name.Contains(name)).ToList();
        }
        public bool isExistedId(int id)
        {
            return _context.Products.Any(x => x.Id == id);
        }
        public void DeleteProduct(int id)
        {
            var inventory = _context.Products.Find(id);
            if (inventory != null)
            {
                _context.Products.Remove(inventory);
                _context.SaveChanges();
            }
        }
        public void AddProduct(Product inventory)
        {
            if (inventory != null)
            {
                _context.Products.Add(inventory);
                _context.SaveChanges();
            }
        }
        public void UpdateProduct(Product inventory)
        {
            if (inventory != null)
            {
                _context.Products.Update(inventory);
                _context.SaveChanges();
            }
        }
    }
}
