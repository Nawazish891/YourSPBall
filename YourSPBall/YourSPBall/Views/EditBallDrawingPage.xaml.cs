using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Models;
using YourSPBall.Resources;

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

                });
            }
        }

        public Command ClearPaintCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.IconClicked();

                    string action = DisplayActionSheet(AppResources.DeleteBallMsg, AppResources.Cancel, null, new string[] { AppResources.No, AppResources.Yes }).Result;

                    if (action != AppResources.Yes)
                        return;

                    if (SPBall.ID > 0)
                        App.Database.DeleteSPBall(SPBall);

                    DisplayAlert("YourSPBall", AppResources.DeleteDataMsg, AppResources.Cancel);
                    App.Current.MainPage = new NavigationPage(new MainMenuPage());
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