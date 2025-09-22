using AutoMapper;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.DTOs.JournalEntry;

namespace FinancialManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ChartOfAccount
            CreateMap<ChartOfAccount, ChartOfAccountDto>()
                .ForMember(dest => dest.ParentAccountName,
                           opt => opt.MapFrom(src => src.ParentAccount != null ? src.ParentAccount.AccountName : null));

            CreateMap<CreateChartOfAccountDto, ChartOfAccount>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.OpeningBalance));

            CreateMap<UpdateChartOfAccountDto, ChartOfAccount>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // JournalEntry
            CreateMap<JournalEntry, JournalEntryDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<CreateJournalEntryDto, JournalEntry>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.JournalNumber, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsPosted, opt => opt.MapFrom(_ => false));

            CreateMap<UpdateJournalEntryDto, JournalEntry>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.JournalNumber, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // JournalEntryLine
            CreateMap<JournalEntryLine, JournalEntryLineDto>()
                .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.Account.AccountCode))
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.AccountName));

            CreateMap<CreateJournalEntryLineDto, JournalEntryLine>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

            CreateMap<UpdateJournalEntryLineDto, JournalEntryLine>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
