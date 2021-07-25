using System;
using Com.Theartofdev.Edmodo.Cropper;
using Android.App;
using Android.Content;
using YourSPBall.ImageCropper;

namespace YourSPBall.Droid.ImageCropper
{
    public class Platform
    {
        public static async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == CropImage.CropImageActivityRequestCode)
            {
                CropImage.ActivityResult result = CropImage.GetActivityResult(data);

                // small delay
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromMilliseconds(100));
                if (resultCode == Result.Ok)
                {
                    YourSPBall.ImageCropper.ImageCropper.Current.Success?.Invoke(result.Uri.Path);
                }
                else if ((int)resultCode == (int)(CropImage.CropImageActivityResultErrorCode))
                {
                    YourSPBall.ImageCropper.ImageCropper.Current.Failure?.Invoke();
                }
            }
        }
    }
}
