using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace TrafficPolice
{
    public partial class ViewDriverWindow : Window
    {
        private const long maxPhotoFileSize = 1024 * 1024 * 2; // 2 Mb
        private static Notifier? _notifierInstance;
        public DriverModel Driver;
        public string PhotoMimeType = "image/png";

        public ViewDriverWindow(DriverModel driver)
        {
            InitializeComponent();
            Driver = driver;

            LoadDriverFromData(Driver);
        }

        public void LoadDriverFromData(DriverModel driver)
        {
            Surname.Text = driver.Surname;
            Name.Text = driver.Name;
            Middlename.Text = driver.MiddleName;
            Passport.Text = driver.Passport;
            RegisterAddress.Text = driver.RegistrationAddress;
            LivingAddress.Text = driver.LivingAddress;
            WorksAs.Text = driver.WorksAs;
            WorksAt.Text = driver.WorksAt;
            PhoneNumber.Text = driver.PhoneNumber;
            Email.Text = driver.Email;

            var imageModel = TrafficPoliceDB.GetPhoto(driver.PhotoId);
            using (var ms = new System.IO.MemoryStream(imageModel.BinaryContent))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                
                Photo.Source = image;
            }

            Notes.Text = driver.Notes;
        }

        private void saveDriverBtn_Click(object sender, RoutedEventArgs e)
        {
            Driver.Surname = Surname.Text;
            Driver.Name = Name.Text;
            Driver.MiddleName = Middlename.Text;
            Driver.Passport = Passport.Text;
            Driver.RegistrationAddress = RegisterAddress.Text;
            Driver.LivingAddress = LivingAddress.Text;
            Driver.WorksAs = WorksAs.Text;
            Driver.WorksAt = WorksAt.Text;
            Driver.PhoneNumber = PhoneNumber.Text;
            Driver.Email = Email.Text;
            Driver.Notes = Notes.Text;

            TrafficPoliceDB.UpdateDriver(Driver);
            Close();
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

        private void Photo_onMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

            PhotoMimeType = "image/" + openFileDialog.FileName.Split('.').Last();

            var newPhoto = new PhotoModel()
            {
                Id = TrafficPoliceDB.GenerateGuid(),
                BinaryContent = File.ReadAllBytes(openFileDialog.FileName),
                MimeType = PhotoMimeType
            };
            
            TrafficPoliceDB.UploadPhoto(newPhoto);
            Driver.PhotoId = newPhoto.Id;

            LoadDriverFromData(Driver);
        }

        private void deleteDriverBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить этого пользователя?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            TrafficPoliceDB.DeleteDriver(Driver);
            Close();
        }
    }
}
