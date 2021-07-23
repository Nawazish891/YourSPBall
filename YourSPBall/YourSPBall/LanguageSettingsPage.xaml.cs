using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Resources;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageSettingsPage : ContentPage
    {
        public LanguageSettingsPage()
        {
            InitializeComponent();
        }

        public Command ChangeLanguageCommand
        {
            get
            {
                return new Command<string>((param)=> {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(param);
                    AppResources.Culture = new CultureInfo(param);
                    App.Current.MainPage = new NavigationPage(new MainMenuPage());
                });
            }
        }
    }
}