using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
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
    /// Логика взаимодействия для WindowAuth.xaml
    /// </summary>
    public partial class WindowAuth : Window
    {
        public WindowAuth()
        {
            InitializeComponent();
        }

        private void BtnAuth_Click(object sender, RoutedEventArgs e)
        {
            Authtorize();
        }

        async private void Authtorize()
        {
            var fireApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            var userRecord = await FirebaseAuth.GetAuth(fireApp)
                .GetUserByEmailAsync(TxtLogin.Text);
            if (userRecord != null)
            {
                var user = await App.db.Collection("Users")
                    .Document(userRecord.Uid).GetSnapshotAsync();
                if (user.GetValue<string>("AdminPass") == TxtPassword.Password 
                    && user.GetValue<Int64>("Role") == 0)
                {
                    App.mainWindow.UserName = user.GetValue<string>("Name") + " " 
                        + user.GetValue<string>("Patronimyc");
                    App.mainWindow.LoadDB();
                    Close();
                }
            }

        }
    }
}
