﻿namespace PlayersBay.Services.Data.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public static class ApplicationCloudinary
    {
        public static async Task<string> UploadImage(Cloudinary cloudinary, IFormFile image, string name)
        {
            byte[] destinationImage;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            using (var ms = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(name, ms),
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.SecureUri.AbsoluteUri;
            }
        }

        public static void DeleteImage(Cloudinary cloudinary, string name)
        {
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
            };

            cloudinary.DeleteResources(delParams);
        }
    }
}
