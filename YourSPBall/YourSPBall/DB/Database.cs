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
            _database.CreateTableAsync<SPBall>().Wait();
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
            if (settings.ID != 0)
                return _database.UpdateAsync(settings);

            return _database.InsertAsync(settings);
        }

        /// <summary>
        /// Get all SpBalls user has created
        /// </summary>
        /// <returns></returns>
        public Task<List<SPBall>> GetAllSPBalls()
        {
            return _database.Table<SPBall>().ToListAsync();
        }


        /// <summary>
        /// Insert or Update SPBall based on ID
        /// </summary>
        /// <param name="spBall"></param>
        /// <returns></returns>
        public Task<int> SaveSPBallAsync(SPBall spBall)
        {
            if (spBall.ID != 0)
                return _database.UpdateAsync(spBall);

            return _database.InsertAsync(spBall);
        }

        /// <summary>
        /// Delete All SpBalls
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteAllSPBalls()
        {
            return _database.DeleteAllAsync<SPBall>();
        }


        /// <summary>
        /// Delete SpBall
        /// </summary>
        /// <param name="spBall"></param>
        /// <returns></returns>
        public Task<int> DeleteSPBall(SPBall spBall)
        {
            return _database.DeleteAsync<SPBall>(spBall.ID);
        }
    }

}
