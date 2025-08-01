using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Service
{
    public class ProductService
    {
        private readonly ProductRepository productRepository;
        public ProductService()
        {
            productRepository = new ProductRepository();
        }
        public List<Product> getAllProducts()
        {
            return productRepository.GetAllProducts();
        }

        public List<Product> findByProductName(string productName)
        {
            return productRepository.findByProductName(productName);
        }

    }
}
