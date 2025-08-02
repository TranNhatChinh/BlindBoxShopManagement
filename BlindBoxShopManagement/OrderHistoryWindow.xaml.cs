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
    /// Interaction logic for OrderHistoryWindow.xaml
    /// </summary>
    public partial class OrderHistoryWindow : Window
    {
        private readonly OrderService _orderService;    
        public OrderHistoryWindow()
        {
            InitializeComponent();
            _orderService = new OrderService(); 
            load();
        }

        public void load()
        {
            dgvDisplay.ItemsSource = _orderService.GetAllOrders();
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.DataContext as DAL.Entities.Order; // sửa OrderViewModel ➝ Order

            if (order != null)
            {
                var detailWindow = new OrderDetailWindow(order.Id); // truyền OrderId
                detailWindow.ShowDialog();
            }
        }

        private void SearchByCustomerNameClick(object sender, RoutedEventArgs e)
        {
            string customerName = txtCustomerName.Text?.Trim();

            // Nếu txtCustomerName rỗng hoặc null, load lại toàn bộ danh sách
            if (string.IsNullOrEmpty(customerName))
            {
                load();
                return;
            }

            // Gọi phương thức tìm kiếm từ OrderService
            var orders = _orderService.GetOrdersByCustomerName(customerName);

            // Kiểm tra nếu không tìm thấy đơn hàng
            if (orders == null || !orders.Any())
            {
                MessageBox.Show("Không tìm thấy đơn hàng nào cho khách hàng này.",
                               "Thông báo",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
                return;
            }

            // Cập nhật DataGrid với danh sách đơn hàng tìm được
            dgvDisplay.ItemsSource = orders;
        }

        private void SearchByDateClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu DatePicker không có ngày được chọn
            if (DpPicker.SelectedDate == null)
            {
                load();
                return;
            }

            // Lấy ngày được chọn
            DateTime selectedDate = DpPicker.SelectedDate.Value;

            // Gọi phương thức tìm kiếm theo ngày từ OrderService
            var orders = _orderService.GetOrdersByDate(selectedDate);

            // Kiểm tra nếu không tìm thấy đơn hàng
            if (orders == null || !orders.Any())
            {
                MessageBox.Show("Không tìm thấy đơn hàng nào cho ngày này.",
                               "Thông báo",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
                return;
            }

            // Cập nhật DataGrid với danh sách đơn hàng tìm được
            dgvDisplay.ItemsSource = orders;
        }
    }
    }

