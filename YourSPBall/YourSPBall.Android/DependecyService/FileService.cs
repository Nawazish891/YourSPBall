using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using YourSPBall.Droid.DependencyService;
using YourSPBall.Interface;

[assembly: Dependency(typeof(FileService))]
namespace YourSPBall.Droid.DependencyService
{
    public class FileService : IFileService
    {
        public string SavePicture(string name, Stream data)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "SPBallImages");
            Directory.CreateDirectory(documentsPath);
            string filePath = Path.Combine(documentsPath, name);
            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }

            return filePath;
        }
    }
}