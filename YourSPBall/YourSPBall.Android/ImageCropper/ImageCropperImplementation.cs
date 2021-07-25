using Com.Theartofdev.Edmodo.Cropper;
using System.Diagnostics;
using System;
using YourSPBall.ImageCropper;
using Android.App;
using Xamarin.Forms;
using YourSPBall.Droid.ImageCropper;

[assembly: Dependency(typeof(ImageCropperImplementation))]

namespace YourSPBall.Droid.ImageCropper
{
    public class ImageCropperImplementation : IImageCropperWrapper
    {
        public void ShowFromFile(YourSPBall.ImageCropper.ImageCropper imageCropper, string imageFile)
        {
            try
            {
                CropImage.ActivityBuilder activityBuilder = CropImage.Activity(Android.Net.Uri.FromFile(new Java.IO.File(imageFile)));

                if (imageCropper.CropShape == YourSPBall.ImageCropper.ImageCropper.CropShapeType.Oval)
                {
                    activityBuilder.SetCropShape(CropImageView.CropShape.Oval);
                }
                else
                {
                    activityBuilder.SetCropShape(CropImageView.CropShape.Rectangle);
                }

                if (imageCropper.AspectRatioX > 0 && imageCropper.AspectRatioY > 0)
                {
                    activityBuilder.SetFixAspectRatio(true);
                    activityBuilder.SetAspectRatio(imageCropper.AspectRatioX, imageCropper.AspectRatioY);
                }
                else
                {
                    activityBuilder.SetFixAspectRatio(false);
                }

                if (!string.IsNullOrWhiteSpace(imageCropper.PageTitle))
                {
                    activityBuilder.SetActivityTitle(imageCropper.PageTitle);
                }

                activityBuilder.Start(Xamarin.Essentials.Platform.CurrentActivity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }
        public byte[] GetBytes(string imageFile)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, imageFile);
            return System.IO.File.ReadAllBytes(filePath);
        }
    }
}
