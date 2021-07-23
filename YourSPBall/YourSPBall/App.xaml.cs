using System;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Resources;

namespace YourSPBall
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
            //AppResources.Culture = new CultureInfo("de");
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
