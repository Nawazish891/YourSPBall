using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Models;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private UserSettings _Settings;
        public UserSettings Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            Settings = App.Database.GetSettings();
        }

        public Command BackCommand
        {
            get
            {
                return new Command(()=> {
                    this.Navigation.PopAsync();
                });
            }
        }
        public Command MuteUnmuteCommand
        {
            get
            {
                return new Command(() => {
                    Settings.MuteSound = !Settings.MuteSound;
                    App.Database.SaveSettingsAsync(Settings).Wait();
                    Settings = App.Database.GetSettings();
                });
            }
        }
    }
}