using DATN_Back_end.Config;
using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoReport;
using DATN_Back_end.Exceptions;
using DATN_Back_end.Extensions;
using DATN_Back_end.Models;
using DATN_Back_end.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;

namespace DATN_Back_end.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly DataContext dataContext;

        private readonly ICloudinaryService cloudinaryService;

        private readonly IAuthenticationService authenticationService;

        private readonly CloudinaryConfig cloudinaryConfig;

        public ReportRepository(DataContext dataContext,
            ICloudinaryService cloudinaryService,
            CloudinaryConfig cloudinaryConfig,
            IAuthenticationService authenticationService) : base(dataContext)
        {
            this.dataContext = dataContext;
            this.cloudinaryService = cloudinaryService;
            this.cloudinaryConfig = cloudinaryConfig;
            this.authenticationService = authenticationService;
        }

        public async Task Create(ReportFormDto reportFormDto)
        {
            reportFormDto.UploadFileLink = await CheckForUploading(reportFormDto.UploadFileLink);

            reportFormDto.CreatedTime = DateTime.Now;

            await base.Create(reportFormDto);
        }

        public async Task<List<ReportItem>> Get()
        {
            return await dataContext.Reports
                .Include(x => x.Comments)
                .Select(x => x.ConvertTo<ReportItem>())
                .ToListAsync();
        }

        public async Task<ReportDetail> Get(Guid id)
        {
            var entry = await dataContext.Reports
                .Include(x => x.Comments)
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .Select(x => x.ConvertTo<ReportDetail>())
                .FirstOrDefaultAsync();

            if (entry == null)
            {
                throw new NotFoundException("Report cannot be found");
            }

            return entry;
        }

        public async Task<List<ReportItem>> GetReportsByDate(ReportsFilter reportsFilter)
        {
            reportsFilter.DateTime = DateTime.Parse(reportsFilter.DateTime.ToString()).ToLocalTime();
            var test = await dataContext.Reports
                .Include(x => x.User).ToListAsync();

            var res = await dataContext.Reports
                .Include(x => x.Comments)
                .Include(x => x.User)
                .ThenInclude(x => x.Department)
                .Where(x => (reportsFilter.DepartmentId.HasValue ? x.User.DepartmentId == reportsFilter.DepartmentId.Value
                : x != null))
                .Where(x => reportsFilter.UserId.HasValue ? x.UserId == reportsFilter.UserId.Value
                : x != null)
                .Where(x => reportsFilter.DateTime.HasValue ?
                (x.CreatedTime.Day == reportsFilter.DateTime.Value.Day
                && x.CreatedTime.Month == reportsFilter.DateTime.Value.Month
                && x.CreatedTime.Year == reportsFilter.DateTime.Value.Year)
                : x != null)
                .Select(x => x.ConvertTo<ReportItem>())
                .ToListAsync();

            return res;
        }

        public async Task<ReportDetail> GetReportsByDateForUser(DateTime dateTime)
        {
            var currentUserId = authenticationService.CurrentUserId;
            return await dataContext.Reports
                .Where(x => x.User.Id == currentUserId
                    && x.CreatedTime.Day == dateTime.Day
                    && x.CreatedTime.Month == dateTime.Month
                    && x.CreatedTime.Year == dateTime.Year)
                .Include(x => x.Comments)
                .Include(x => x.User)
                .ThenInclude(x => x.Department)
                .Select(x => x.ConvertTo<ReportDetail>())
                .FirstOrDefaultAsync();
        }

        public async Task<List<ReportItem>> GetReportsByUserId(Guid id)
        {
            var currentUserId = id;

            return await dataContext.Reports
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.Comments)
                .Include(x => x.User)
                .ThenInclude(x => x.Department)
                .Select(x => x.ConvertTo<ReportItem>())
                .ToListAsync();
        }

        public async Task Update(Guid id, ReportFormDto reportFormDto)
        {
            reportFormDto.UploadFileLink = await CheckForUploading(reportFormDto.UploadFileLink);

            reportFormDto.UpdatedTime = DateTime.Now;

            await base.Update(id, reportFormDto);
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

            var dataUpload = img.ImageToByteArray();

            var uploadResult = await cloudinaryService.UploadImage(imageName, dataUpload);

            return uploadResult;
        }
    }
}
