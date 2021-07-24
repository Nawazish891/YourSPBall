using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Models;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBallDrawingPage : ContentPage
    {
        #region -------Ctor------
        public EditBallDrawingPage(SPBall spBall = null)
        {
            InitializeComponent();
            
            if(spBall == null)
                spBall = new SPBall();

            SPBall = spBall;
            PreviousSPBall = new SPBall()
            {
                ImageURL = SPBall.ImageURL,
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
                return new Command(async () => {
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
                                SPBall.ImageURL = imageFile;
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
                return new Command(() => {
                    App.IconClicked();
                    
                });
            }
        }

        public Command ClearPaintCommand
        {
            get
            {
                return new Command(() => {
                    App.IconClicked();
                    App.Database.DeleteSPBall(SPBall);
                    App.Current.MainPage = new NavigationPage(new MainMenuPage());
                });
            }
        }

        public Command SaveSPBallCommand
        {
            get
            {
                return new Command(() => {
                    App.IconClicked();
                    App.Database.SaveSPBallAsync(SPBall);
                    this.Navigation.PopAsync();
                });
            }
        }
        #endregion
    }
}