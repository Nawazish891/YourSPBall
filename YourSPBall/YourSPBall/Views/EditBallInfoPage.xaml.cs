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
    public partial class EditBallInfoPage : ContentPage
    {
        Task animateTask;
        public EditBallInfoPage(SPBall spBall)
        {
            InitializeComponent();
            SPBall = spBall;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            animateTask = new Task(() => { RotateImageContinously(); });
            animateTask.RunSynchronously();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            animateTask.Dispose();
            animateTask = null;
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
        #endregion
    }
}