using BAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlindBoxShopManagement
{
    /// <summary>
    /// Interaction logic for OrderDetailWindow.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window
    {
        private readonly OrderDetailService _orderDetailService;
        private int _orderId; // Biến để lưu Id của đơn hàng
        public OrderDetailWindow(int orderId)
        {
            InitializeComponent();
            _orderDetailService = new OrderDetailService();
            _orderId = orderId;
            load();  // Gọi hàm load để hiển thị dữ liệu

            // Khởi tạo service
        }

        public void load()
        {
          dgvOrderDetails.ItemsSource = _orderDetailService.GetOrderDetailsByOrderId(_orderId); 
        }
    }
}
