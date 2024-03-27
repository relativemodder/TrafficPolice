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

        private void Login()
        {
            try
            {
                App.CurrentUserID = TrafficPoliceDB.Login(usernameTextBox.Text, passwordBox.Password);
            }

            catch (LoginExceptions.IncorrectUsernameOrPasswordException)
            {
                App.GetNotifierInstance().ShowError("Неверный логин или пароль!");
                return;
            }

            catch (LoginExceptions.CooldownException cooldown)
            {
                App.GetNotifierInstance()
                    .ShowError(
                        "Вы неправильно ввели логин/пароль 3 раза. " 
                        + $"Повторите попытку через {cooldown.RemainingTime} сек."
                    );
                return;
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
