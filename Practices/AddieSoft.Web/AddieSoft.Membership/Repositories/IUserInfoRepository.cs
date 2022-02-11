using AddieSoft.Membership.Contexts;
using AddieSoft.Membership.Entities;
using DevSkill.Data;

namespace AddieSoft.Membership.Repositories
{
    public interface IUserInfoRepository : IRepository<UserInfo, int, MembershipDbContext>
    {
    }
}