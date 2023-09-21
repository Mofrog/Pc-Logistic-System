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
    /// Логика взаимодействия для UCUserItem.xaml
    /// </summary>
    public partial class UCUserItem : UserControl
    {
        public UCUserItem(DocumentSnapshot snapshot)
        {
            InitializeComponent();

            txtId.Text = snapshot.Id;
            Dictionary<string, object> documentDictionary = snapshot.ToDictionary();
            if (int.Parse(documentDictionary["Role"].ToString()) == 0) return;

            txtRole.Text = App.Roles[int.Parse(documentDictionary["Role"].ToString())];
            txtName.Text = (string)documentDictionary["Name"];
            txtPatronimyc.Text = (string)documentDictionary["Patronimyc"];
            txtSurname.Text = (string)documentDictionary["Surname"];
            txtVehicle.Text = (string)documentDictionary["Vehicle"];
        }
    }
}
