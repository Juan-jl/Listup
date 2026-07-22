using Listup.Models;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Listup.SQLiteData
{
    public static class DatabaseConnection
    {
        private static SQLiteAsyncConnection _connection;

        public static SQLiteAsyncConnection GetConnection()
        {
            if (_connection == null)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "listupDB.db3");
                _connection = new SQLiteAsyncConnection(dbPath);
            }

            return _connection;
        }

        public static async Task InitializeAsync()
        {
            var db = GetConnection();
            await db.CreateTableAsync<Cart>();
            await db.CreateTableAsync<CartItem>();
            await db.CreateTableAsync<Configs>();
        }
    }
}
