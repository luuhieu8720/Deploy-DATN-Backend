using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public interface ICloudinaryService
    {
        public Task<string> UploadFile(IFormFile file);

        Task<string> UploadImage(string imageName, byte[] data);
    }
}
