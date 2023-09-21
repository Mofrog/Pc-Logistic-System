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
using System.Windows.Shapes;

namespace LogisticSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowRoutePrint.xaml
    /// </summary>
    public partial class WindowRoutePrint : Window
    {
        private void AddCell(string text, int row, int column)
        {
            var txtBlock = new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap
            };
            Data.Children.Add(txtBlock);
            txtBlock.SetValue(Grid.RowProperty, row);
            txtBlock.SetValue(Grid.ColumnProperty, column);
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1.0)
            };
            Data.Children.Add(border);
            border.SetValue(Grid.RowProperty, row);
            border.SetValue(Grid.ColumnProperty, column);
        }

        async private void GetOrders(string user)
        {
            CollectionReference ordersRef = App.db.Collection("Orders");
            CollectionReference usersRef = App.db.Collection("Users");
            IReadOnlyList<DocumentSnapshot> snapshotOrders = (await ordersRef.GetSnapshotAsync()).Documents;
            IReadOnlyList<DocumentSnapshot> snapshotUsers = (await usersRef.GetSnapshotAsync()).Documents;
            var userData = snapshotUsers.Where(x => x.Id == user).First().ToDictionary();
            txtUser.Text = "Работник: " + userData["Surname"] + " " + userData["Name"] + " " + userData["Patronimyc"];

            for (int i = 0; i < App.SelectedOrders.Count; i++)
            {
                var orderRef = await App.db.Collection("Orders").Document(App.SelectedOrders[i]).GetSnapshotAsync();
                var row = new RowDefinition();
                row.Height = new GridLength(60.0);
                Data.RowDefinitions.Add(row);

                AddCell((i + 1).ToString(), i + 1, 0);
                AddCell(App.Customers[(int)orderRef.GetValue<Int64>("CustomerId")], i + 1, 1);
                AddCell(orderRef.GetValue<string>("Address"), i + 1, 2);
                AddCell((string)userData["Vehicle"], i + 1, 3);
                AddCell(orderRef.GetValue<string>("OrderDate"), i + 1, 4);
                AddCell("", i + 1, 5);
            }
        }

        public WindowRoutePrint(string user)
        {
            InitializeComponent();
            DateTime dateTime = DateTime.Now;
            txtCurrentDate.Text = 
                "Маршрутный лист выдан: «" + dateTime.Day.ToString() + "» 0" 
                + dateTime.Month.ToString()  + "  " 
                + dateTime.Year.ToString() + "г.  в " 
                + dateTime.Hour.ToString() + "ч " 
                + dateTime.Minute.ToString() + "мин.";
            GetOrders(user);
        }
    }
}
