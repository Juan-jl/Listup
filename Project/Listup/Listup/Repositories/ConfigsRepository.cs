using Listup.SQLiteData;
using SQLite;
using System.Threading.Tasks;
using Listup.Models;

namespace Listup.Repositories
{
    public class ConfigsRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public ConfigsRepository()
        {
            _db = DatabaseConnection.GetConnection();
        }

        public async Task<int> InsertAsync()
        {
            if (await CountConfigs() == 0)
            {
                var config = new Configs
                {
                    CurrentLanguageId = "pt-br",
                    DefaultCurrencyCode = "BRL",
                    IsUserOnboarding = true
                };
                return await _db.InsertAsync(config);
            }
            return 0;
        }

        public async Task<Configs> GetConfigRecordAsync()
        {
            return await _db.FindAsync<Configs>(1);
        }

        public async Task<int> UpdateAsync(Configs config)
        {
            return await _db.UpdateAsync(config);
        }

        public Task<int> CountConfigs()
        {
            return _db.Table<Configs>().CountAsync();
        }
    }
}
