using Google.Cloud.Firestore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogisticSystem.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UCOrderItemSelect.xaml
    /// </summary>
    public partial class UCOrderItemSelect : UserControl
    {
        private string orderId;
        private bool isSelected = false;

        public UCOrderItemSelect(DocumentSnapshot snapshot)
        {
            InitializeComponent();
            orderId = snapshot.Id;
            Dictionary<string, object> documentDictionary = snapshot.ToDictionary();
            txtAddress.Text = (string)documentDictionary["Address"];
            txtCity.Text = (string)documentDictionary["City"];
            txtOrderDate.Text = (string)documentDictionary["OrderDate"];
            txtCustomer.Text = App.Customers[int.Parse(documentDictionary["CustomerId"].ToString())];
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (isSelected)
            {
                BtnSelect.Content = "Выбрать";
                App.SelectedOrders.Remove(orderId);
                isSelected = false;
            }
            else
            {
                BtnSelect.Content = "Отменить";
                App.SelectedOrders.Add(orderId);
                isSelected = true;
            }
        }
    }
}
