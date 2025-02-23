using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Application
{
    public class GlobalMapping : Profile
    {
        public GlobalMapping()
        {
            CreateMap<DtoModelBase, DataModelBase>()
                .ForMember(dst => dst.Code, opts => opts.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<PersonDto, Person>()
                .IncludeBase<DtoModelBase, DataModelBase>()
                .ForMember(dst => dst.Name, opts => opts.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.Surname, opts => opts.MapFrom(src => src.LastName))
                .ForMember(dst => dst.IdNumber, opts => opts.MapFrom(src => src.IdNo))
                .ForMember(dst => dst.Accounts, opts => opts.Ignore());
            CreateMap<Person, PersonDto>()
                .IncludeBase<DataModelBase, DtoModelBase>()
                .ForMember(dst => dst.FirstName, opts => opts.MapFrom(src => src.Name))
                .ForMember(dst => dst.LastName, opts => opts.MapFrom(src => src.Surname))
                .ForMember(dst => dst.IdNo, opts => opts.MapFrom(src => src.IdNumber));

            CreateMap<AccountDto, Account>()
                .IncludeBase<DtoModelBase, DataModelBase>()
                .ForMember(dst => dst.AccountNumber, opts => opts.MapFrom(src => src.AccountNo))
                .ForMember(dst => dst.PersonCode, opts => opts.MapFrom(src => src.UserId))
                .ReverseMap();
            CreateMap<TransactionDto, Transaction>()
                .IncludeBase<DtoModelBase, DataModelBase>()
                .ForMember(dst => dst.TransactionDate, opts => opts.MapFrom(src => src.OrderedDate))
                .ForMember(dst => dst.CaptureDate, opts => opts.MapFrom(src => src.ProcessedDate))
                .ForMember(dst => dst.AccountCode, opts => opts.MapFrom(src => src.AccountId))
                .ForMember(dst => dst.Amount, opts => opts.MapFrom(src => src.Total))
                .ReverseMap();
        }
    }
}