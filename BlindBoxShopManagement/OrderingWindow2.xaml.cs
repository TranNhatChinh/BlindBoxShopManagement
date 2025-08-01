using BAL.Service;
using DAL.Entities;
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
    /// Interaction logic for OrderingWindow2.xaml
    /// </summary>
    public partial class OrderingWindow2 : Window
    {
        private readonly ProductService productService;
        public List<Product> SelectedProducts { get; private set; } // Khai báo thuộc tính để trả về danh sách

        public OrderingWindow2()
        {
            InitializeComponent();
            productService = new ProductService();
            SelectedProducts = new List<Product>(); // Khởi tạo danh sách
        }

        public void load()
        {
            List<Product> products = productService.getAllProducts();
            dgvDisplay.ItemsSource = products;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgvDisplay.SelectedItems;
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SelectedProducts.Clear(); // Xóa danh sách cũ
            foreach (Product item in selectedItems)
            {
                SelectedProducts.Add(item);
            }

            DialogResult = true; // Đánh dấu cửa sổ đóng thành công
            Close();
        }
    }
}