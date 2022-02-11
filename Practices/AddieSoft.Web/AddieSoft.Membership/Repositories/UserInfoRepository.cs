using AddieSoft.Membership.Contexts;
using AddieSoft.Membership.Entities;
using DevSkill.Data;

namespace AddieSoft.Membership.Repositories
{
    public class UserInfoRepository : Repository<UserInfo, int, MembershipDbContext>, IUserInfoRepository
    {
        public UserInfoRepository(IMembershipDbContext dbContext)
            :base((MembershipDbContext)dbContext)
        {

        }
    }
}