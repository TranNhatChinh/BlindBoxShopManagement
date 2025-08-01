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
using ViewModels;

namespace BlindBoxShopManagement
{
    /// <summary>
    /// Interaction logic for OrderingWindow.xaml
    /// </summary>
    public partial class OrderingWindow : Window
    {
        private List<OrderItemViewModel> orderItems = new List<OrderItemViewModel>();

        public OrderingWindow()
        {
            InitializeComponent();
        }

        private void SaveOrderClick(object sender, RoutedEventArgs e)
        {

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

                dgvOrderItems.ItemsSource = null;
                dgvOrderItems.ItemsSource = orderItems;
            }
        }

        private void RemoveItemClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
