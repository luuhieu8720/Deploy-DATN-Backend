using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IReportRepository
    {
        Task<List<ReportItem>> Get();

        Task<ReportDetail> Get(Guid id);

        Task Create(ReportFormDto reportFormDto);

        Task Update(Guid id, ReportFormDto reportFormDto);

        Task<List<ReportItem>> GetReportsByUserId(Guid id);

        Task<ReportDetail> GetReportsByDateForUser(DateTime dateTime);

        Task<List<ReportItem>> GetReportsByDate(ReportsFilter reportsFilter);
    }
}
