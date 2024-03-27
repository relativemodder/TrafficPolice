using System.Windows;
using System.Windows.Controls;

namespace TrafficPolice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoginPage loginPage = new LoginPage();
            navFrame.Content = loginPage;
        }

        public static MainWindow GetShared()
        {
            return (MainWindow)Application.Current.MainWindow;
        }

        public static Frame GetNavigationFrame()
        {
            return GetShared().navFrame;
        }

        public void TriggerActivityCounter()
        {
            // TODO: Logout when no activity
            // MessageBox.Show("Activity Triggered!");
        }

        private void onKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TriggerActivityCounter();
        }

        private void onMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TriggerActivityCounter();
        }
    }
}