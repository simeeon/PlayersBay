namespace PlayersBay.Services.Data.Utilities
{
    public static class ImageFormat
    {
        public static bool IsImageTypeValid(string fileType)
        {
            return fileType == DataConstants.JpgFormat || fileType == DataConstants.PngFormat || fileType == DataConstants.JpegFormat;
        }
    }
}
