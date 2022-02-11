using AutoMapper;
using EO = AddieSoft.Membership.Entities;
using BO = AddieSoft.Membership.BusinessObjects;

namespace AddieSoft.Membership.Profiles
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<EO.UserInfo, BO.UserInfo>().ReverseMap();
        }
    }
}