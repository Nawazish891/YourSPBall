using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourSPBall.Resources;

namespace YourSPBall
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBallDrawingPage : ContentPage
    {
        public EditBallDrawingPage()
        {
            InitializeComponent();
        }
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
                                imgView.Source = ImageSource.FromFile(imageFile);
                            });
                        }
                    }.Show(this);
                });
            }
        }
    }
}