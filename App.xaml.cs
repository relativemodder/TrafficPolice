using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using SQLite;
using System.Diagnostics;

namespace TrafficPolice
{
    public partial class App : Application
    {
        public static int CurrentUserID = 0;

        private static Notifier? _notifierInstance;

        private void Application_onStartup(object sender, StartupEventArgs e)
        {
            var db = TrafficPoliceDB.GetSQLiteConnection();
            TrafficPoliceDB.CreateDefaultUser();
        }

        public static Notifier GetNotifierInstance()
        {
            if (_notifierInstance != null) return _notifierInstance;

            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Current.Dispatcher;
            });

            _notifierInstance = notifier;

            return _notifierInstance;
        }
    }

}
