using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using DATN_Back_end.Models;
using DATN_Back_end.Dto.DtoUser;
using DATN_Back_end.Extensions;
using DATN_Back_end.Config;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using DATN_Back_end.Services;
using System.Collections.Generic;
using DATN_Back_end.Exceptions;

namespace DATN_Back_end.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DataContext dataContext;

        private readonly ICloudinaryService cloudinaryService;

        private readonly CloudinaryConfig cloudinaryConfig;

        public UserRepository(DataContext dataContext, CloudinaryConfig cloudinaryConfig,
            ICloudinaryService cloudinaryService) : base(dataContext)
        {
            this.dataContext = dataContext;
            this.cloudinaryConfig = cloudinaryConfig;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task Create(UserFormCreate userForm)
        {
            userForm.Password = userForm.Password.Encrypt();

            var entry = await dataContext.Departments
                .Where(x => userForm.DepartmentId.HasValue ? x.Id == userForm.DepartmentId.Value : x == null)
                .FirstOrDefaultAsync();

            if (entry != null)
            {
                if (userForm.Role == Role.Manager && entry.ManagerId.HasValue)
                {
                    throw new BadRequestException("This department already have a manager");
                }
            }

            if (userForm.Role == Role.Manager && 
                (userForm.DepartmentId.ToString() == "" || userForm.DepartmentId == null))
            {
                throw new BadRequestException("Manager must be placed in at least 1 department");
            }

            await base.Create(userForm);
        }

        public async Task Update(Guid id, UserFormUpdate userForm)
        {
            userForm.AvatarUrl = await CheckForUploading(userForm.AvatarUrl);

            var user = await Get(userForm.Email);

            var department = await dataContext.Departments.FindAsync(userForm.DepartmentId.Value);

            if (userForm.Role == Role.Manager && user.Id != department.ManagerId)
            {
                throw new BadRequestException("This department already has a manager");
            }

            if (userForm.Role == Role.Manager && userForm.DepartmentId.ToString() == "")
            {
                throw new BadRequestException("Manager must be placed in at least 1 department");
            }

            await base.Update(id, userForm);
        }

        public async Task<UserDetail> Get(Guid Id)
        {
            return await dataContext.Users
                .Include(x => x.Department)
                .Where(x => x.Id == Id)
                .Select(x => x.ConvertTo<UserDetail>())
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetById(Guid Id)
        {
            return await dataContext.Users.FindAsync(Id);
        }

        public async Task<User> Get(string email)
        {
            return await dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        private async Task<string> CheckForUploading(string urlOrBase64)
        {
            if (string.IsNullOrEmpty(urlOrBase64))
            {
                return string.Empty;
            }

            if (new Regex("http(s)?://").IsMatch(urlOrBase64))
            {
                return urlOrBase64;
            }

            var base64Header = "base64,";
            var imageHeader = "image/";
            var fileExtension = urlOrBase64.Substring(urlOrBase64.IndexOf(imageHeader) + imageHeader.Length, urlOrBase64.IndexOf(base64Header) - base64Header.Length);

            var imageName = Guid.NewGuid().ToString("N") + "." + fileExtension;
            var imageData = urlOrBase64.Substring(urlOrBase64.IndexOf(base64Header) + base64Header.Length);
            var fileData = Convert.FromBase64String(imageData);

            //using var memoryStream = new MemoryStream(fileData);
            var img = Image.Load<Rgba32>(fileData);

            var resizedImage = img.EnsureLimitSize(cloudinaryConfig.CoverLimitHeight, cloudinaryConfig.CoverLimitWidth);

            var dataUpload = resizedImage.ImageToByteArray();

            var uploadResult = await cloudinaryService.UploadImage(imageName, dataUpload);

            return uploadResult;
        }

        public async Task<List<UserItem>> GetAll()
        {
            return await dataContext.Users
                .Include(x => x.Department)
                .Select(x => x.ConvertTo<UserItem>())
                .ToListAsync();
        }

        public async Task<List<UserItem>> GetUserByDepartmentId(Guid id)
        {
            return await dataContext.Users
                .Include(x => x.Department)
                .Where(x => x.DepartmentId == id)
                .Select(x => x.ConvertTo<UserItem>())
                .ToListAsync();
        }
    }
}
