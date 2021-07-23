using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Xamarin.Forms;
using YourSPBall.DB;
using YourSPBall.Resources;

namespace YourSPBall
{
    public partial class App : Application
    {
        static Database database;
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YourSPBall.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            string langCode = App.Database.GetSettings().SelectedLanguageCode;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);
            AppResources.Culture = new CultureInfo(langCode);
            MainPage = new NavigationPage(new MainMenuPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
