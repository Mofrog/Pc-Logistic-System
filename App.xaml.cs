using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LogisticSystem
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Dictionary<int, string> Statuses = new Dictionary<int, string>();
        public static Dictionary<int, string> Roles = new Dictionary<int, string>();
        public static Dictionary<int, string> Customers = new Dictionary<int, string>();
        public static Dictionary<string, string> Users = new Dictionary<string, string>();
        public static List<string> SelectedOrders = new List<string>();

        public static FirestoreDb db;

        public static MainWindow mainWindow;
    }
}
