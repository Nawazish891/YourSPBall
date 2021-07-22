using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BallDetailScreen : ContentPage
    {
        public BallDetailScreen()
        {
            InitializeComponent();
        }

        public Command BackCommand
        {
            get
            {
                return new Command(() => {
                    this.Navigation.PopAsync();
                });
            }
        }

        public Command NavigateToEditCommand
        {
            get
            {
                return new Command(() => {
                    this.Navigation.PushAsync(new EditBallInfoPage());
                });
            }
        }

        public Command NavigateToEditDrawingCommand
        {
            get
            {
                return new Command(() => {
                    this.Navigation.PushAsync(new EditBallDrawingPage());
                });
            }
        }
        
    }
}