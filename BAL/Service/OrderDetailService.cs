using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Service
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepository;  
        public OrderDetailService()
        {
            _orderDetailRepository = new OrderDetailRepository(); // Khởi tạo repository
        }   
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            // Sử dụng repository để lấy danh sách OrderDetail theo OrderId
            return _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }   
    }
}
