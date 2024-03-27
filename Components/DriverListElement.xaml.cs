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

namespace TrafficPolice
{
    public partial class DriverListElement : UserControl
    {
        public delegate void OnSelect(DriverModel driver);
        public event OnSelect? Select;
        public DriverModel Driver;

        public DriverListElement(DriverModel driver)
        {
            InitializeComponent();
            Driver = driver;

            stringRepresentation.Text = driver.ToString();
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            Select?.Invoke(Driver);
        }
    }
}
