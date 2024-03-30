using System.Text;
using SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace TrafficPolice
{
    public class TrafficPoliceDB
    {
        private const string defaultUsername = "inspector";
        private const string defaultPassword = "inspector";
        private const int loginMaxAttempts = 3;
        private const int loginCooldown = 60; // in seconds

        private static SQLiteConnection? _instance;
        private static SQLiteAsyncConnection? _asyncInstance;
        private static Random rnd = new Random();

        public static SQLiteConnection GetSQLiteConnection()
        {
            if (_instance == null)
            {
                var databasePath = Path.Combine(Environment.CurrentDirectory, "db.sqlite");

                if (!File.Exists(databasePath))
                {
                    File.Create(databasePath);
                }

                var db = new SQLiteConnection(databasePath);

                db.CreateTable<PhotoModel>();
                db.CreateTable<DriverModel>();
                db.CreateTable<InspectorModel>();
                db.CreateTable<LoginAttempt>();

                _instance = db;
            }

            return _instance;
        }

        public static List<DriverModel> GetDrivers()
        {
            var conn = GetSQLiteConnection();
            var query = conn.Table<DriverModel>();

            var driversList = query.ToList();
            return driversList;
        }

        public static void UpdateDriver(DriverModel driver)
        {
            var conn = GetSQLiteConnection();
            conn.Update(driver);
        }

        public static void CreateDefaultUser()
        {
            try
            {
                GetSQLiteConnection().Insert(new InspectorModel()
                {
                    Guid = GenerateGuid(),
                    Username = defaultUsername,
                    Password = HashPassword(defaultPassword)
                });
            }
            catch (SQLiteException)
            {
                Debug.WriteLine($"User already exists, no need to insert.");
            }
        }

        public static void UploadPhoto(PhotoModel photo)
        {
            var db = GetSQLiteConnection();
            db.Insert(photo);
        }

        public static void InsertDriver(DriverModel driver)
        {
            var db = GetSQLiteConnection();
            db.Insert(driver);
        }

        public static string HashPassword(string password)
        {
            return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(password))).ToLower();
        }

        public static bool VerifyPassword(string hash, string password)
        {
            return HashPassword(password) == hash;
        }

        public static long GetUnixTimestamp()
        {
            long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTimestamp;
        }

        public static int GenerateGuid()
        {
            return rnd.Next(111111111, 999999999);
        }

        public static int Login(string username, string password)
        {
            var db = GetSQLiteConnection();

            var deltaTime = GetUnixTimestamp() - loginCooldown;

            var attemptsQuery = db.Table<LoginAttempt>().Where(attempt => attempt.Timestamp > deltaTime);
            
            if (attemptsQuery.Count() > loginMaxAttempts)
            {
                var lastAttempt = attemptsQuery.LastOrDefault();
                long remainingTime = lastAttempt.Timestamp + loginCooldown - GetUnixTimestamp();
                throw new LoginExceptions.CooldownException() { RemainingTime = (int)remainingTime };
            }

            var userQuery = db.Table<InspectorModel>().Where(inspector => inspector.Username == username);
            var user = userQuery.FirstOrDefault();

            var attemptGuid = GenerateGuid();

            var insertedAttempt = db.Insert(new LoginAttempt()
            {
                Id = attemptGuid,
                Timestamp = GetUnixTimestamp()
            });


            if (user == null)
            {
                throw new LoginExceptions.IncorrectUsernameOrPasswordException();
            }


            if (VerifyPassword(user.Password, password) == false)
            {
                throw new LoginExceptions.IncorrectUsernameOrPasswordException();
            }

            db.Delete<LoginAttempt>(attemptGuid);
            db.Commit();

            return user.Guid;
        }

        public static PhotoModel GetPhoto(int id)
        {
            var db = GetSQLiteConnection();
            var photoQuery = db.Table<PhotoModel>().Where(photo => photo.Id == id);
            return photoQuery.FirstOrDefault();
        }
    }
}
