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
    public partial class BallDetailScreen : ContentPage
    {
        Task animateTask;
        #region ----Ctor----
        public BallDetailScreen(SPBall spBall)
        {
            InitializeComponent();
            SPBall = spBall;
            animateTask = new Task(()=> { RotateImageContinously(); });
            animateTask.RunSynchronously();
        }
        #endregion

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
        #endregion

        #region ----Commands----

        public Command BackCommand
        {
            get
            {
                return new Command(() => {
                    App.IconClicked();
                    this.Navigation.PopAsync();
                });
            }
        }

        public Command NavigateToEditCommand
        {
            get
            {
                return new Command(() => {
                    App.IconClicked();
                    this.Navigation.PushAsync(new EditBallInfoPage(SPBall));
                });
            }
        }

        public Command NavigateToEditDrawingCommand
        {
            get
            {
                return new Command(() => {
                    App.IconClicked();
                    this.Navigation.PushAsync(new EditBallDrawingPage(SPBall));
                });
            }
        }
        #endregion
    }
}