
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Models;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {

        #region ----Ctor----
        public MainMenuPage()
        {
            InitializeComponent();
        }
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var t = Task.Run(() => App.Database.GetAllSPBalls());
            t.Wait();
            SPBalls = t.Result;
            CurrentBall = SPBalls != null && SPBalls.Count > 0 ? SPBalls.FirstOrDefault() : null;
        }

        #region ----Properties----
        private List<SPBall> _lstSPBalls;
        public List<SPBall> SPBalls
        {
            get { return _lstSPBalls; }
            set { _lstSPBalls = value; OnPropertyChanged(); }
        }

        private SPBall _currentBall;
        public SPBall CurrentBall
        {
            get { return _currentBall; }
            set { _currentBall = value; OnPropertyChanged(); }
        }

        #endregion

        #region ----Commands----

        public Command NavigateToSettingsCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    this.Navigation.PushAsync(new SettingsPage());
                });
            }
        }

        public Command NavigateToLangSettingsCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    this.Navigation.PushAsync(new LanguageSettingsPage());
                });
            }
        }

        public Command NavigateToBallDetailCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    this.Navigation.PushAsync(new BallDetailScreen(CurrentBall));
                });
            }
        }
        public Command NavigateToBallDrawingCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    this.Navigation.PushAsync(new EditBallDrawingPage());
                });
            }
        }
        public Command ChangeBall
        {
            get
            {
                return new Command<string>((param) =>
                {
                    if (SPBalls == null || SPBalls.Count == 0)
                        return;

                    App.IconClicked();

                    if (param.ToLower() == "next")
                    {
                        if (CurrentBall != SPBalls.Last())
                            CurrentBall = SPBalls.Where(x=>x.ID == (CurrentBall.ID + 1)).FirstOrDefault();
                        else if (CurrentBall == SPBalls.Last())
                            CurrentBall = SPBalls.FirstOrDefault();
                    }
                    else
                    {
                        if (CurrentBall != SPBalls.First())
                            CurrentBall = SPBalls.Where(x => x.ID == (CurrentBall.ID - 1)).FirstOrDefault();
                        else if (CurrentBall == SPBalls.First())
                            CurrentBall = SPBalls.Last();
                    }

                });
            }
        }
        #endregion
    }
}