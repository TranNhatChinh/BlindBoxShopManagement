using BAL.Service;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ViewModels;

namespace BlindBoxShopManagement
{
    /// <summary>
    /// Interaction logic for OrderingWindow.xaml
    /// </summary>
    public partial class OrderingWindow : Window
    {
        private ObservableCollection<OrderItemViewModel> orderItems = new ObservableCollection<OrderItemViewModel>();
        private OrderService orderService = new OrderService();
        public OrderingWindow()
        {
            InitializeComponent();

            orderItems.CollectionChanged += OrderItems_CollectionChanged;

        }

        private void SaveOrderClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra thông tin cơ bản
                if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtCustomerName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPhoneNumber.Focus();
                    return;
                }

                if (!orderItems.Any())
                {
                    MessageBox.Show("Vui lòng thêm sản phẩm vào đơn hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra stock trước khi lưu
                var orderItemsForCheck = orderItems.Select(item => (item.ProductName, item.Quantity)).ToList();
                var insufficientProducts = orderService.CheckStockAvailability(orderItemsForCheck);

                if (insufficientProducts.Any())
                {
                    string message = "Không đủ hàng trong kho cho các sản phẩm sau:\n\n" +
                                   string.Join("\n", insufficientProducts) +
                                   "\n\nVui lòng điều chỉnh số lượng hoặc loại bỏ sản phẩm này khỏi đơn hàng.";

                    MessageBox.Show(message, "Không đủ hàng trong kho", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tạo đối tượng Order
                var order = new Order
                {
                    CustomerName = txtCustomerName.Text.Trim(),
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    CreatedAt = DateTime.Now,
                    TotalPrice = orderItems.Sum(item => item.TotalPrice),
                    OrderDetails = orderItems.Select(item => new OrderDetail
                    {
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.TotalPrice
                    }).ToList()
                };

                // Lưu order
                bool success = orderService.AddOrder(order);

                if (success)
                {
                    MessageBox.Show("Đơn hàng đã được lưu thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reset form sau khi lưu thành công
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi lưu đơn hàng. Vui lòng thử lại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            OrderingWindow2 orderingWindow2 = new OrderingWindow2();
            if (orderingWindow2.ShowDialog() == true)
            {
                foreach (var product in orderingWindow2.SelectedProducts)
                {
                    // Nếu sản phẩm đã có trong danh sách, tăng số lượng
                    var existing = orderItems.FirstOrDefault(x => x.ProductName == product.Name);
                    if (existing != null)
                    {
                        existing.Quantity += 1;
                    }
                    else
                    {
                        orderItems.Add(new OrderItemViewModel
                        {
                            ProductName = product.Name,
                            Price = product.Price,
                            Quantity = 1
                        });
                    }
                }

                dgvOrderItems.ItemsSource = orderItems;
            }
        }

        private void RemoveItemClick(object sender, RoutedEventArgs e)
        {
        }

        private void OrderItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (OrderItemViewModel item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (OrderItemViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }

            UpdateGrandTotal();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemViewModel.Quantity) || e.PropertyName == nameof(OrderItemViewModel.TotalPrice))
            {
                UpdateGrandTotal();
            }
        }

        private void UpdateGrandTotal()
        {
            decimal grandTotal = orderItems.Sum(item => item.TotalPrice);
            lblGrandTotal.Content = grandTotal.ToString("N0") + " đ";
        }

        private void ResetForm()
        {
            txtCustomerName.Clear();
            txtPhoneNumber.Clear();
            orderItems.Clear();
            dgvOrderItems.ItemsSource = null;
            UpdateGrandTotal();
        }
    }
}
