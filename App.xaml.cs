using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace TrafficPolice
{
    public partial class App : Application
    {
        private void Application_onStartup(object sender, StartupEventArgs e)
        {
            TrafficPoliceDB.GetSQLiteConnection();
        }

        public static Notifier GetNotifierInstance()
        {
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

            return notifier;
        }
    }

}
