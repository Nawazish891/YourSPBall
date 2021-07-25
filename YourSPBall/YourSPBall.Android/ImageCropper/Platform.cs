using System;
using Com.Theartofdev.Edmodo.Cropper;
using Android.App;
using Android.Content;
using YourSPBall.ImageCropper;
using Android.Graphics;
using YourSPBall.Droid.DependencyService;
using System.IO;
using Java.IO;

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
                    YourSPBall.ImageCropper.ImageCropper.Current.Success?.Invoke(getClip(result.Uri.Path));
                }
                else if ((int)resultCode == (int)(CropImage.CropImageActivityResultErrorCode))
                {
                    YourSPBall.ImageCropper.ImageCropper.Current.Failure?.Invoke();
                }
            }
        }

        /// <summary>
        /// Convert Square Image to Circle
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string getClip(string imagePath)
        {
            Bitmap bitmap = BitmapFactory.DecodeFile(imagePath);
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width,
                bitmap.Height,Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);
            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            canvas.DrawCircle(bitmap.Width / 2f, bitmap.Height / 2f, bitmap.Width / 2f, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rect, rect, paint);
            FileService fileS = new FileService();
            return fileS.SavePicture(imagePath.Replace(".jpg",".png"), output);
        }
    }
}
