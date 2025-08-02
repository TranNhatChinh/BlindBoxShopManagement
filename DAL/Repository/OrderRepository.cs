using DAL.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class OrderRepository
    {
        private readonly BlindBoxShopContext _context;
        public OrderRepository()
        {
            _context = new BlindBoxShopContext();
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }   

        public List<Order> GetOrdersByCustomerName(string customerName)
        {
            return _context.Orders.Where(o => o.CustomerName.Contains(customerName)).ToList();
        }

        public List<Order> GetOrdersByDate(DateTime date)
        {
            return _context.Orders.Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == date.Date).ToList();
        }

        public void AddOrder(Order order)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Thêm order
                _context.Orders.Add(order);
                _context.SaveChanges();

                // Cập nhật stock cho từng sản phẩm
                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Name == orderDetail.ProductName);
                    if (product != null)
                    {
                        product.Stock -= orderDetail.Quantity;
                        _context.Products.Update(product);
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool CheckStockAvailability(List<(string ProductName, int Quantity)> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.Name == item.ProductName);
                if (product == null || product.Stock < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public List<string> GetInsufficientStockProducts(List<(string ProductName, int Quantity)> orderItems)
        {
            var insufficientProducts = new List<string>();

            foreach (var item in orderItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.Name == item.ProductName);
                if (product == null)
                {
                    insufficientProducts.Add($"{item.ProductName} (không tồn tại)");
                }
                else if (product.Stock < item.Quantity)
                {
                    insufficientProducts.Add($"{item.ProductName} (còn {product.Stock}, cần {item.Quantity})");
                }
            }

            return insufficientProducts;
        }
    }
}
