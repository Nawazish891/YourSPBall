using Plugin.SharedTransitions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Models;
using YourSPBall.Resources;
using YourSPBall.Views;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBallDrawingPage : ContentPage
    {
        #region -------Ctor------
        public EditBallDrawingPage(SPBall spBall = null)
        {
            InitializeComponent();

            if (spBall == null)
                spBall = new SPBall();

            SPBall = spBall;
            PreviousSPBall = new SPBall()
            {
                ID = SPBall.ID,
                ImageURL = SPBall.ImageURL,
                Name = SPBall.Name,
                SportType = SPBall.SportType
            };
        }
        #endregion

        #region -----Properties-----
        private SPBall previousSPBall;
        public SPBall PreviousSPBall
        {
            get
            {
                return previousSPBall;
            }
            set
            {
                previousSPBall = value;
                OnPropertyChanged();
            }
        }

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
        public Command PickImageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    App.IconClicked();
                    await new YourSPBall.ImageCropper.ImageCropper()
                    {
                        CropShape = YourSPBall.ImageCropper.ImageCropper.CropShapeType.Oval,
                        AspectRatioX = 1,
                        AspectRatioY = 1,
                        Success = (imageFile) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                PreviousSPBall.ImageURL = imageFile;
                            });
                        }
                    }.Show(this);
                });
            }
        }
        public Command PaintImageCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();
                    this.Navigation.PushAsync(new SpinPaintScreen(PreviousSPBall));
                });
            }
        }

        public Command ClearPaintCommand
        {
            get
            {
                return new Command(async() =>
                {
                    App.IconClicked();
                    if (string.IsNullOrEmpty(SPBall.ImageURL))
                        return;

                    string action = await DisplayActionSheet(AppResources.DeleteBallMsg, AppResources.Cancel, null, new string[] { AppResources.No, AppResources.Yes });

                    if (action != AppResources.Yes)
                        return;

                    if (SPBall.ID > 0)
                        await App.Database.DeleteSPBall(SPBall);

                    await DisplayAlert("YourSPBall", AppResources.DeleteSuccessfull, AppResources.OK, AppResources.Cancel);
                    App.Current.MainPage = new SharedTransitionNavigationPage(new MainMenuPage());
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
                    App.Database.SaveSPBallAsync(PreviousSPBall);
                    this.Navigation.PushAsync(new EditBallInfoPage(PreviousSPBall));
                });
            }
        }
        #endregion
    }
}