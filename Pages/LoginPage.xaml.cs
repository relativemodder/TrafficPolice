using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications.Messages;

namespace TrafficPolice
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void ShowLoginError()
        {
            App.GetNotifierInstance().ShowError("Неверный логин или пароль!");
        }

        private void Login()
        {
            var db = TrafficPoliceDB.GetSQLiteConnection();
            var query = db.Table<InspectorModel>().Where(inspector => inspector.Username == usernameTextBox.Text);

            var result = query.FirstOrDefault();

            if (result == null)
            {
                ShowLoginError();
                return;
            }

            var passwordHash = TrafficPoliceDB.HashPassword(passwordBox.Password);

            if (result.Password != passwordHash)
            {
                ShowLoginError();
            }

            var mainPage = new MainPage();
            MainWindow.GetNavigationFrame().Content = mainPage;
        }

        private void passwordBox_onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void loginButton_onClick(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
