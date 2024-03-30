using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications.Messages;

namespace TrafficPolice
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void LoadDriversAsync()
        {
            var driversList = TrafficPoliceDB.GetDrivers();

            foreach (var driver in driversList)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    AddDriverToListView(driver);
                }));
            }
        }

        private void ReloadDriversList()
        {
            driversListView.Items.Clear();
            Task.Run(() =>
            {
                LoadDriversAsync();
            });
        }

        private DriverListElement AddDriverToListView(DriverModel driver)
        {
            var driverListElement = new DriverListElement(driver);
            driverListElement.HorizontalAlignment = HorizontalAlignment.Stretch;
            driverListElement.Select += DriverListElement_Select;
            driversListView.Items.Add(driverListElement);

            return driverListElement;
        }

        private void DriverListElement_Select(DriverModel driver)
        {
            var viewDriverWindow = new ViewDriverWindow(driver);
            viewDriverWindow.Show();
        }

        private void createDriverBtn_onClick(object sender, RoutedEventArgs e)
        {
            CreateDriverWindow createDriverWindow = new CreateDriverWindow();
            createDriverWindow.OnOK += CreateDriverWindow_OnOK;
            createDriverWindow.ShowDialog();
        }

        private void CreateDriverWindow_OnOK(CreateDriverWindow w)
        {
            bool validationError = false;

            if (w.Surname.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Фамилия\"!");
                validationError = true;
            }

            if (w.Name.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Имя\"!");
                validationError = true;
            }

            if (w.Middlename.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Отчество\"!");
                validationError = true;
            }

            if (w.Passport.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Паспорт\"!");
                validationError = true;
            }

            if (w.RegistrationAddress.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Адрес регистрации\"!");
                validationError = true;
            }

            if (w.LivingAddress.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Адрес проживания\"!");
                validationError = true;
            }

            if (w.PhoneNumber.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"Номер телефона\"!");
                validationError = true;
            }

            if (w.Email.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Заполните поле \"E-mail\"!");
                validationError = true;
            }

            if (w.Email.Text.Contains('@') == false)
            {
                w.GetNotifierInstance().ShowError("Поле \"E-mail\" не заполнено правильно!");
                validationError = true;
            }

            if (w.Photo.Text == string.Empty)
            {
                w.GetNotifierInstance().ShowError("Выберите фотографию!");
                validationError = true;
            }

            if (validationError)
            {
                return;
            }

            var photo = new PhotoModel()
            {
                Id = TrafficPoliceDB.GenerateGuid(),
                BinaryContent = File.ReadAllBytes(w.Photo.Text),
                MimeType = w.PhotoMimeType
            };

            TrafficPoliceDB.UploadPhoto(photo);

            DriverModel driver = new DriverModel()
            {
                Guid = TrafficPoliceDB.GenerateGuid(),
                Surname = w.Surname.Text,
                Name = w.Name.Text,
                MiddleName = w.Middlename.Text,
                Passport = w.Passport.Text,
                RegistrationAddress = w.RegistrationAddress.Text,
                LivingAddress = w.LivingAddress.Text,
                WorksAt = w.WorksAt.Text != null ? w.WorksAt.Text : null,
                WorksAs = w.WorksAs.Text != null ? w.WorksAs.Text : null,
                PhoneNumber = w.PhoneNumber.Text,
                Email = w.Email.Text,
                PhotoId = photo.Id,
                Notes = w.Notes.Text != null ? w.Notes.Text : null
            };

            TrafficPoliceDB.InsertDriver(driver);

            w.Close();

            ReloadDriversList();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadDriversList();
        }

        private void reloadBtn_onClick(object sender, RoutedEventArgs e)
        {
            ReloadDriversList();
        }
    }
}
