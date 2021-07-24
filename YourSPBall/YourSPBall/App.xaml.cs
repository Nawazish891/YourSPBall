using Plugin.SimpleAudioPlayer;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
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

        public static bool MuteSound;
        public App()
        {
            InitializeComponent();
            string langCode = App.Database.GetSettings().SelectedLanguageCode;
            MuteSound = App.Database.GetSettings().MuteSound;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);
            AppResources.Culture = new CultureInfo(langCode);
            ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;
            player.Load(GetStreamFromFile("background.mp3"));
            player.Loop = true;
            player.Play();

            if (MuteSound)
                player.Pause();
            MainPage = new NavigationPage(new MainMenuPage());
        }

        public static void IconClicked()
        {
            if (MuteSound)
                return;

            var player1 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player1.Load(GetStreamFromFile("buttonclick.mp3"));
            player1.Loop = false;
            player1.Play();
        }
        static Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("YourSPBall.Music." + filename);
            return stream;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;
            player.Pause();
        }

        protected override void OnResume()
        {
            ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;

            if (!MuteSound)
                player.Play();
        }
    }
}
