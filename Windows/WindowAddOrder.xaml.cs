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

namespace LogisticSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowAddOrder.xaml
    /// </summary>
    public partial class WindowAddOrder : Window
    {

        async private void AddItem()
        {
            var docRef = App.db.Collection("Orders");
            Dictionary<string, object> order = new Dictionary<string, object>
            {
                { "Address", TxtAddress.Text },
                { "City", TxtCity.Text },
                { "Comment", TxtComment.Text },
                { "CustomerId", CBCustomer.SelectedIndex },
                { "OrderDate", OrderDate.SelectedDate.ToString() },
                { "Status", 0 },
                { "CompleteDate", "" },
                { "TakeDate", "" },
                { "UserId", "" }
            };
            await docRef.AddAsync(order);
            App.mainWindow.LoadDB();
        }

        public WindowAddOrder()
        {
            InitializeComponent();

            foreach (var customer in App.Customers)
            {
                var item = new ComboBoxItem();
                item.Content = customer.Value;
                CBCustomer.Items.Add(item);
                CBCustomer.SelectedIndex = 0;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
            Close();
        }
    }
}
