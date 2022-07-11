using AutoMapper;
using DATN_Back_end.Dto.DtoComment;
using DATN_Back_end.Dto.DtoDepartment;
using DATN_Back_end.Dto.DtoFormRequest;
using DATN_Back_end.Dto.DtoReport;
using DATN_Back_end.Dto.DtoStatus;
using DATN_Back_end.Dto.DtoTimeKeeping;
using DATN_Back_end.Dto.DtoUser;
using DATN_Back_end.Dto.DtoWorkingTime;
using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public static class MapperService
    {
        private static readonly MapperConfiguration config = new(CreateMap);
        private static readonly IMapper mapper = config.CreateMapper();

        private static void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, UserItem>()
                .ForMember(user => user.Department,
                option => option.MapFrom(user => user.Department.ConvertTo<DepartmentShorted>()));
            cfg.CreateMap<User, UserDetail>()
                .ForMember(user => user.Department,
                    option => option.MapFrom(user => user.Department.ConvertTo<DepartmentShorted>()));
            cfg.CreateMap<User, UserShorted>();
            cfg.CreateMap<UserDetail, User>();
            cfg.CreateMap<UserFormCreate, User>();
            cfg.CreateMap<UserFormUpdate, User>();

            cfg.CreateMap<Department, DepartmentDetail>();
            cfg.CreateMap<DepartmentShorted, Department>();
            cfg.CreateMap<Department, DepartmentShorted>();
            cfg.CreateMap<Department, DepartmentItem>()
                .ForMember(departmentItem => departmentItem.Manager,
                option => option.MapFrom(department => department.Manager.ConvertTo<UserShorted>()));
            cfg.CreateMap<DepartmentForm, Department>();

            cfg.CreateMap<FormRequest, FormRequestItem>()
                .ForMember(request => request.User,
                option => option.MapFrom(request => request.User))
                .ForMember(request => request.RequestType,
                option => option.MapFrom(request => request.RequestType));

            cfg.CreateMap<FormRequest, FormRequestDetail>()
                .ForMember(request => request.User,
                option => option.MapFrom(request => request.User))
                .ForMember(request => request.RequestType,
                option => option.MapFrom(request => request.RequestType));

            cfg.CreateMap<FormRequestForm, FormRequest>();
            cfg.CreateMap<FormRequestConfirm, FormRequest>();

            cfg.CreateMap<FormStatus, FormStatusDetail>();
            cfg.CreateMap<FormStatus, FormStatusItem>();
            cfg.CreateMap<FormStatusForm, FormStatus>();

            cfg.CreateMap<CommentForm, Comment>();
            cfg.CreateMap<Comment, CommentDetail>()
                .ForMember(comment => comment.CommentedUser,
                option => option.MapFrom(comment => comment.CommentedUser));
            cfg.CreateMap<Comment, CommentItem>()
                .ForMember(comment => comment.CommentedUser,
                option => option.MapFrom(comment => comment.CommentedUser));
            cfg.CreateMap<CommentItem, Comment>();

            cfg.CreateMap<Report, ReportItem>()
                .ForMember(reportItem => reportItem.Comments,
                option => option.MapFrom(book => book.Comments.Select(x => x.ConvertTo<CommentItem>())))
                .ForMember(reportItem => reportItem.User,
                option => option.MapFrom(book => book.User));

            cfg.CreateMap<Report, ReportDetail>()
                .ForMember(reportItem => reportItem.Comments,
                option => option.MapFrom(book => book.Comments.Select(x => x.ConvertTo<CommentItem>())))
                .ForMember(reportItem => reportItem.User,
                option => option.MapFrom(book => book.User));

            cfg.CreateMap<ReportForm, Report>();
            cfg.CreateMap<ReportForm, ReportFormDto>();
            cfg.CreateMap<ReportFormDto, Report>();

            cfg.CreateMap<TimeKeepingForm, Timekeeping>();
            cfg.CreateMap<Timekeeping, TimeKeepingItem>()
                .ForMember(timekeeping => timekeeping.User,
                option => option.MapFrom(timekeeping => timekeeping.User));
            cfg.CreateMap<Timekeeping, TimeKeepingDetail>()
                .ForMember(timekeeping => timekeeping.User,
                option => option.MapFrom(timekeeping => timekeeping.User));
        }

        public static T ConvertTo<T>(this object source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }

            return mapper.Map<T>(source);
        }
        public static void CopyTo(this object source, object destination)
        {
            if (source == null)
            {
                throw new NullReferenceException("Source can't be null");
            }

            if (destination == null)
            {
                throw new NullReferenceException("Destination can't be null");
            }

            mapper.Map(source, destination);
        }
    }
}
