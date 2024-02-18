using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsBanking.Common.Dto
{
    public class CustomerDto
    {
        public string FullName { get; set; }
        public long CitizenNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
    }

    public class CustomerResponseDto:CustomerDto
    {
        public string Id { get; set; }
    }
    public class CustomerDtoProfile : Profile
    {
        public CustomerDtoProfile()
        {
            CreateMap<CustomerDto, MsBanking.Common.Entity.Customer>().ReverseMap();
            CreateMap<CustomerResponseDto, MsBanking.Common.Entity.Customer>().ReverseMap();
        }
    }
}
