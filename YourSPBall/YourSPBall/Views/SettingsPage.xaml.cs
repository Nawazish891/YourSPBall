using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Models;
using YourSPBall.Resources;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        #region ----Properties----
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
        #endregion

        #region ----Ctor----
        public SettingsPage()
        {
            InitializeComponent();
            Settings = App.Database.GetSettings();
        }
        #endregion

        #region ----Commands----
        public Command BackCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    this.Navigation.PopAsync();
                });
            }
        }
        public Command MuteUnmuteCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    Settings.MuteSound = !Settings.MuteSound;
                    App.Database.SaveSettingsAsync(Settings).Wait();
                    Settings = App.Database.GetSettings();
                    ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;
                    App.MuteSound = Settings.MuteSound;
                    if (Settings.MuteSound)
                        player.Pause();
                    else
                        player.Play();
                });
            }
        }

        public Command DeleteDataCommand
        {
            get
            {
                return new Command(async () =>
                {
                    App.IconClicked();
                    string action = await DisplayActionSheet(AppResources.DeleteDataMsg, null, null, new string[] { AppResources.No, AppResources.Yes });

                    if (action == AppResources.Yes)
                        await App.Database.DeleteAllSPBalls();

                    await DisplayAlert("YourSPBall", AppResources.DeleteAllSuccessfull, AppResources.Cancel);
                });
            }
        }
        #endregion
    }
}