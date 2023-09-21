using Google.Cloud.Firestore;
using LogisticSystem.UserControls;
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
    /// Логика взаимодействия для WindowCreateRoute.xaml
    /// </summary>
    public partial class WindowCreateRoute : Window
    {
        IReadOnlyList<DocumentSnapshot> snapshotOrders;
        IReadOnlyList<DocumentSnapshot> snapshotUsers;

        /// <summary>
        /// Формирует список наименее занятых водителей для составления маршрутного листа
        /// </summary>
        async private void LoadData()
        {
            CollectionReference ordersRef = App.db.Collection("Orders");
            CollectionReference usersRef = App.db.Collection("Users");
            snapshotOrders = (await ordersRef.GetSnapshotAsync()).Documents;
            snapshotUsers = (await usersRef.GetSnapshotAsync()).Documents;
            var userOrders = new Dictionary<string, int>();
            // Составляет массив всех водителей
            foreach (var user in snapshotUsers)
            {
                if (user.GetValue<Int64>("Role") == 1) userOrders.Add(user.Id, 0);
                foreach (var order in snapshotOrders)
                {
                    if (user.Id == order.GetValue<string>("UserId") 
                        && order.GetValue<Int64>("Status") == 0)
                    {
                        userOrders[user.Id]++;
                    }
                }
            }
            // Формирует список водителей, с учетом количества заказов
            foreach (var user in userOrders.OrderBy(p => p.Value))
            {
                if (user.Value <= 4)
                {
                    var item = new ComboBoxItem();
                    item.Content = App.Users[user.Key] + " " 
                        + user.Value + " активных заказов.";
                    item.Tag = user.Key;
                    CBUser.Items.Add(item);
                }
            }
            foreach (DocumentSnapshot document in snapshotOrders)
            {
                if ((string)document.ToDictionary()["UserId"] == "")
                {
                    Orders.Children.Add(new UCOrderItemSelect(document));
                }
            }
        }

        async private void SetRoute()
        {
            foreach (var order in App.SelectedOrders)
            {
                Google.Cloud.Firestore.DocumentReference orderRef = App.db.Collection("Orders").Document(order);
                Dictionary<string, object> update = new Dictionary<string, object>{{ "UserId", ((ComboBoxItem)CBUser.SelectedItem).Tag }};
                await orderRef.SetAsync(update, SetOptions.MergeAll);
            }
        }

        public WindowCreateRoute()
        {
            InitializeComponent();
            LoadData();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            WindowRoutePrint windowRoutePrint = new WindowRoutePrint((string) ((ComboBoxItem)CBUser.SelectedItem).Tag);
            windowRoutePrint.ShowDialog();
            windowRoutePrint.Activate();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            SetRoute();
            App.mainWindow.LoadDB();
            Close();
        }
    }
}
