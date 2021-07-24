namespace YourSPBall.ImageCropper
{
    public interface IImageCropperWrapper
    {
        void ShowFromFile(ImageCropper imageCropper, string imageFile);

        byte[] GetBytes(string imageFile);
    }
}