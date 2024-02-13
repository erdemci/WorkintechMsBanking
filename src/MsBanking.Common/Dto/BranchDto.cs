using AutoMapper;

namespace MsBanking.Common.Dto
{
    public class BranchDto
    {
        public string Name { get; set; }
        public int CityCode { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class BranchResponseDto : BranchDto
    {
        public int Id { get; set; }
    }

    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<MsBanking.Common.Entity.Branch,BranchDto>()
                .ForMember(x=>x.CityName,opt=>opt.MapFrom(src=>Enum.GetName(typeof(CityEnum), src.CityId)))
                .ForMember(x=>x.CountryName,opt=>opt.MapFrom(src=>Enum.GetName(typeof(CountryEnum), src.CountryId)))
               .ReverseMap();
            CreateMap<MsBanking.Common.Entity.Branch, BranchResponseDto>()
                .ForMember(x => x.CityName, opt => opt.MapFrom(src => Enum.GetName(typeof(CityEnum), src.CityId)))
                .ForMember(x => x.CountryName, opt => opt.MapFrom(src => Enum.GetName(typeof(CountryEnum), src.CountryId)))
                .ReverseMap();
        }
    }   
} 
