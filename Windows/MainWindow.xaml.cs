using LogisticSystem.Windows;
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
using Google.Cloud.Firestore;
using LogisticSystem.UserControls;

namespace LogisticSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isOrders = true;
        public string UserName = null;

        async public void LoadDB()
        {
            if (UserName == null) return;
            
            TxtAuth.Text = "Вы вошли как: " + UserName;
            DataList.Children.Clear();
            if (isOrders)
            {
                OrdersPanel.Visibility = Visibility.Visible;
                UsersPanel.Visibility = Visibility.Collapsed;
                CollectionReference ordersRef = App.db.Collection("Orders");
                QuerySnapshot snapshot = await ordersRef.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    DataList.Children.Add(new UCOrderItem(document));
                }
            }
            else
            {
                OrdersPanel.Visibility = Visibility.Collapsed;
                UsersPanel.Visibility = Visibility.Visible;
                CollectionReference ordersRef = App.db.Collection("Users");
                QuerySnapshot snapshot = await ordersRef.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.GetValue<Int64>("Role") == 1)
                    {
                        DataList.Children.Add(new UCUserItem(document));
                    }
                }
            }
        }

        async private void LoadEnums()
        {
            QuerySnapshot snapshotStatuses = await App.db.Collection("OrderStatuses").GetSnapshotAsync();
            QuerySnapshot snapshotCustomers = await App.db.Collection("Customers").GetSnapshotAsync();
            QuerySnapshot snapshotRoles = await App.db.Collection("Roles").GetSnapshotAsync();
            QuerySnapshot snapshotUsers = await App.db.Collection("Users").GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshotStatuses.Documents)
            {
                App.Statuses.Add(int.Parse(document.Id), (string)document.ToDictionary()["Name"]);
            }
            foreach (DocumentSnapshot document in snapshotCustomers.Documents)
            {
                App.Customers.Add(int.Parse(document.Id), (string)document.ToDictionary()["Name"]);
            }
            foreach (DocumentSnapshot document in snapshotRoles.Documents)
            {
                App.Roles.Add(int.Parse(document.Id), (string)document.ToDictionary()["Name"]);
            }
            foreach (DocumentSnapshot document in snapshotUsers.Documents)
            {
                App.Users.Add(document.Id, (string)document.ToDictionary()["Name"] + " " 
                    + (string)document.ToDictionary()["Patronimyc"]);
            }

            LoadDB();
        }

        public MainWindow()
        {
            InitializeComponent();
            App.db = FirestoreDb.Create("logistic-system-36d4f");
            LoadEnums();
            App.mainWindow = this;
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            isOrders = true;
            LoadDB();
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            isOrders = false;
            LoadDB();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            WindowAddOrder windowAddOrder = new WindowAddOrder();
            windowAddOrder.ShowDialog();
            windowAddOrder.Activate();
        }

        private void BtnCreateRoute_Click(object sender, RoutedEventArgs e)
        {
            WindowCreateRoute windowCreateRoute = new WindowCreateRoute();
            windowCreateRoute.ShowDialog();
            windowCreateRoute.Activate();
        }

        private void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            WindowAuth windowAuth = new WindowAuth();
            windowAuth.ShowDialog();
            windowAuth.Activate();
        }
    }
}
