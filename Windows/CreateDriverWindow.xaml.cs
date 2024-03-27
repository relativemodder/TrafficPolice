using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace TrafficPolice
{
    public partial class CreateDriverWindow : Window
    {
        public delegate void OnOKDelegate(CreateDriverWindow window);
        public event OnOKDelegate OnOK;

        private const long maxPhotoFileSize = 1024 * 1024 * 2; // 2 Mb
        private static Notifier? _notifierInstance;
        public string PhotoMimeType = "image/png";

        public CreateDriverWindow()
        {
            InitializeComponent();

            var mainWindow = MainWindow.GetShared();

            Left = mainWindow.Left + (mainWindow.Width / 2) - (Width / 2);
            Top = mainWindow.Top + (mainWindow.Height / 2) - (Height / 2);
        }

        private void createDriverBtn_onClick(object sender, RoutedEventArgs e)
        {
            OnOK?.Invoke(this);
        }

        private void selectPhotoBtn_onClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

            var result = openFileDialog.ShowDialog();

            if (result == false) return;

            long length = new FileInfo(openFileDialog.FileName).Length;

            if (length > maxPhotoFileSize)
            {
                GetNotifierInstance().ShowError("Размер файла превышает 2 Мб!");
                return;
            }

            BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
            
            if (bitmapImage.PixelHeight < bitmapImage.PixelWidth)
            {
                GetNotifierInstance().ShowError("Изображение не вертикальное!");
                return;
            }

            Photo.Text = openFileDialog.FileName;
            PhotoMimeType = "image/" + openFileDialog.FileName.Split('.').Last();
        }

        public Notifier GetNotifierInstance()
        {
            if (_notifierInstance != null) return _notifierInstance;

            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: this,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            _notifierInstance = notifier;

            return _notifierInstance;
        }
    }
}
