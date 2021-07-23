using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YourSPBall.Models;

namespace YourSPBall.DB
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UserSettings>().Wait();
        }

        /// <summary>
        /// Get User Settings for Sound and Language
        /// </summary>
        /// <returns></returns>
        public UserSettings GetSettings()
        {
            UserSettings settings = _database.Table<UserSettings>().FirstOrDefaultAsync().Result;

            if (settings == null)
                settings = new UserSettings();

            return settings;
        }

        /// <summary>
        /// Insert or Update Settings based on ID
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Task<int> SaveSettingsAsync(UserSettings settings)
        {
            if(settings.ID != 0)
                return _database.UpdateAsync(settings);

            return _database.InsertAsync(settings);
        }
    }

}
