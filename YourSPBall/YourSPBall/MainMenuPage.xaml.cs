
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        public Command NavigateToSettingsCommand
        {
            get
            {
                return new Command(() =>
                {
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
                    this.Navigation.PushAsync(new BallDetailScreen());
                });
            }
        }
        public Command NavigateToBallDrawingCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.Navigation.PushAsync(new EditBallDrawingPage());
                });
            }
        }
    }
}