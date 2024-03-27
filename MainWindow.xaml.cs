using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}