using Plugin.SharedTransitions;
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
    public partial class EditBallInfoPage : ContentPage
    {
        Task animateTask;
        public EditBallInfoPage(SPBall spBall)
        {
            InitializeComponent();
            SPBall = spBall;

            if (string.IsNullOrEmpty(SPBall.Name))
                Name = AppResources.GiveItAName;
            else
                Name = SPBall.Name;

            if (string.IsNullOrEmpty(SPBall.SportType))
                SportType = AppResources.SportType;
            else
                SportType = SPBall.SportType;

            animateTask = new Task(() => { RotateImageContinously(); });
            animateTask.RunSynchronously();
        }

        async Task RotateImageContinously()
        {
            while (true)
            {
                for (int i = 1; i < 7; i++)
                {
                    if (imgView.Rotation >= 360f) imgView.Rotation = 0;
                    await imgView.RotateTo(i * (360 / 6), 1000, Easing.Linear);
                }
            }
        }

        #region ----Properties----
        private SPBall _SPBall;
        public SPBall SPBall
        {
            get
            {
                return _SPBall;
            }
            set
            {
                _SPBall = value;
                OnPropertyChanged();
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _SportType;
        public string SportType
        {
            get
            {
                return _SportType;
            }
            set
            {
                _SportType = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ----Commmands----

        public Command EditInfoCommand
        {
            get
            {
                return new Command<string>(async (param) =>
                {
                    if (param.ToLower() == "name")
                    {
                        string result = await DisplayPromptAsync("YourSPBall", AppResources.EnterName, AppResources.OK, AppResources.Cancel, AppResources.NameField, 15, null, SPBall.Name);

                        if (!string.IsNullOrEmpty(result))
                        {
                            Name = result;
                            SPBall.Name = result;
                        }
                    }
                    else
                    {
                        string result = await DisplayPromptAsync("YourSPBall", AppResources.EnterSportType, AppResources.OK, AppResources.Cancel, AppResources.SportTypeField, 15, null, SPBall.SportType);
                        if (!string.IsNullOrEmpty(result))
                        {
                            SportType = result;
                            SPBall.SportType = result;
                        }
                    }
                });
            }
        }
        public Command SaveSPBallCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    App.Database.SaveSPBallAsync(SPBall);
                    App.Current.MainPage = new SharedTransitionNavigationPage(new MainMenuPage());
                });
            }
        }
        #endregion
    }
}