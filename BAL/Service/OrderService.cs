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



    }
}
