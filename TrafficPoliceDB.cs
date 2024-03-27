using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Security.Cryptography;

namespace TrafficPolice
{
    public class TrafficPoliceDB
    {
        private static SQLiteConnection? _instance;

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

                db.CreateTable<DriverModel>();
                db.CreateTable<InspectorModel>();

                _instance = db;
            }

            return _instance;
        }

        public static string HashPassword(string password)
        {
            return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(password))).ToLower();
        }
    }
}
