using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToastNotifications.Messages;

namespace TrafficPolice
{
    public partial class LoginPage : Page
    {
        private const int defaultUserId = 208342350; // DONT DO LIKE THAT IN REAL PROJECTS PLEASE
        private const bool skipLogin = true;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void AfterLogin(int userId)
        {
            App.CurrentUserID = userId;

            var mainPage = new MainPage();
            MainWindow.GetNavigationFrame().Content = mainPage;
        }

        private void Login()
        {
            try
            {
                AfterLogin(TrafficPoliceDB.Login(usernameTextBox.Text, passwordBox.Password));
            }

            catch (LoginExceptions.IncorrectUsernameOrPasswordException)
            {
                App.GetNotifierInstance().ShowError("Неверный логин или пароль!");
            }

            catch (LoginExceptions.CooldownException cooldown)
            {
                App.GetNotifierInstance()
                    .ShowError(
                        "Вы неправильно ввели логин/пароль 3 раза. " 
                        + $"Повторите попытку через {cooldown.RemainingTime} сек."
                    );
            }
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

        private void loginPage_loaded(object sender, RoutedEventArgs e)
        {
            if (skipLogin)
            {
                AfterLogin(defaultUserId);
            }
        }
    }
}
