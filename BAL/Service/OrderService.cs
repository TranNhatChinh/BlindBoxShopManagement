using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Service
{
    public class OrderService
    {
        private readonly OrderRepository orderRepository;

        public OrderService()
        {
            orderRepository = new OrderRepository();
        }

        public List<Order> GetAllOrders()
        {
            return orderRepository.GetAllOrders();
        }

        public List<Order> GetOrdersByCustomerName(string customerName)
        {
            return orderRepository.GetOrdersByCustomerName(customerName);
        }

        public bool AddOrder(Order order)
        {
            try
            {
                // Kiểm tra stock trước khi lưu
                var orderItems = order.OrderDetails.Select(od => (od.ProductName, od.Quantity)).ToList();

                if (!orderRepository.CheckStockAvailability(orderItems))
                {
                    return false;
                }

                orderRepository.AddOrder(order);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<string> CheckStockAvailability(List<(string ProductName, int Quantity)> orderItems)
        {
            return orderRepository.GetInsufficientStockProducts(orderItems);
        }

        public List<Order> GetOrdersByDate(DateTime date)
        {
            return orderRepository.GetOrdersByDate(date);



        }
    }
}
