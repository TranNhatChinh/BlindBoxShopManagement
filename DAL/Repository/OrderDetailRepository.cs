using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class OrderDetailRepository
    {
        private readonly BlindBoxShopContext _context;  

        public OrderDetailRepository()
        {
            _context = new BlindBoxShopContext(); // Khởi tạo context
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            // Truy vấn để lấy danh sách OrderDetail theo OrderId
            return _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        }
    }
}
