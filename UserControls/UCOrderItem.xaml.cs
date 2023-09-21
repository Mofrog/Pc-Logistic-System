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
    /// Логика взаимодействия для UCOrderItem.xaml
    /// </summary>
    public partial class UCOrderItem : UserControl
    {
        public UCOrderItem(DocumentSnapshot snapshot)
        {
            InitializeComponent();
            txtId.Text = snapshot.Id;
            Dictionary<string, object> documentDictionary = snapshot.ToDictionary();
            txtAddress.Text = (string)documentDictionary["Address"];
            txtCompleteDate.Text = (string)documentDictionary["CompleteDate"];
            txtOrderDate.Text = (string)documentDictionary["OrderDate"];
            txtTakeDate.Text = (string)documentDictionary["TakeDate"];
            txtCustomer.Text = App.Customers[int.Parse(documentDictionary["CustomerId"].ToString())];
            txtStatus.Text = App.Statuses[int.Parse(documentDictionary["Status"].ToString())];
            if (App.Users.ContainsKey(documentDictionary["UserId"].ToString()))
            {
                txtUser.Text = App.Users[documentDictionary["UserId"].ToString()];
            }
            else
            {
                txtUser.Text = "Отсутствует";
            }
        }
    }
}
