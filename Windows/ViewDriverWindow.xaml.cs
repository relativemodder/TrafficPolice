using System.Windows;

namespace TrafficPolice
{
    public partial class ViewDriverWindow : Window
    {

        public DriverModel Driver;

        public ViewDriverWindow(DriverModel driver)
        {
            InitializeComponent();
            Driver = driver;


        }
    }
}
