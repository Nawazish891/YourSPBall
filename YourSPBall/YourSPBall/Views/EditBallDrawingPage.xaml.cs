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
                    var result = await DisplayActionSheet("YourSPBall", "Cancel", null, new string[] { AppResources.TakeAPicture, AppResources.ChooseFromLibrary });

                    if (result.ToLower() == AppResources.TakeAPicture.ToLower())
                    {
                        var result2 = await MediaPicker.CapturePhotoAsync();

                        if (result2 != null)
                        {
                            var stream = await result2.OpenReadAsync();

                            var imageResult = ImageSource.FromStream(() => stream);
                        }
                    }
                    else if (result.ToLower() == AppResources.ChooseFromLibrary.ToLower())
                    {
                        var result2 = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                        {
                            Title = AppResources.ChooseFromLibrary
                        });

                        if (result2 != null)
                        {
                            var stream = await result2.OpenReadAsync();
                            var imageResult = ImageSource.FromStream(() => stream);
                        }
                    }
                });
            }
        }
    }
}