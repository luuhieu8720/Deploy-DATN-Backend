using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATN_Back_end.Config;
using DATN_Back_end.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private CloudinaryConfig cloudinaryConfig;

        private Cloudinary cloudinary;

        public CloudinaryService(CloudinaryConfig cloudinaryConfig)
        {
            this.cloudinaryConfig = cloudinaryConfig;

            var account = new Account(
                cloudinaryConfig.Cloudinary,
                cloudinaryConfig.ApiKey,
                cloudinaryConfig.ApiSecret);

            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImage(string imageName, byte[] data)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageName, new MemoryStream(data)),
                UseFilename = true,
                UniqueFilename = false
            };

            cloudinary.Api.Secure = true;
            var result = await cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            if (file.Length == 0) throw new BadRequestException("Empty file(s)");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var fileBytes = stream.ToArray();
            string s = Convert.ToBase64String(fileBytes);
            var uploadData = new MemoryStream(Convert.FromBase64String(s));

            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, uploadData)
            };

            cloudinary.Api.Secure = true;

            var result = await cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
    }
}
