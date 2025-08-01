using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repository;

namespace BAL.Service
{
    public class ProductService
    {
        private readonly ProductRepository productRepository;
        public ProductService()
        {
            productRepository = new ProductRepository();
        }
        public List<Product> GetAllProducts()
        {
            return productRepository.GetAllProduct();
        }
        public List<Product> GetAllProductsByProductName(string supplier)
        {
            return productRepository.GetAllProductsByProductName(supplier);
        }
        public bool IsExistedId(int id)
        {
            return productRepository.isExistedId(id);
        }
        public void DeleteProduct(int id)
        {
            productRepository.DeleteProduct(id);
        }
        public void AddProduct(Product product)
        {
            if (product != null)
            {
                productRepository.AddProduct(product);
            }
        }
        public void UpdateProduct(Product product)
        {
            if (product != null)
            {
                productRepository.UpdateProduct(product);
            }
        }
    }
}
