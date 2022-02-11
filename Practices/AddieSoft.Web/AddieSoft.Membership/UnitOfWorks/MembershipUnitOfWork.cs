using AddieSoft.Membership.Contexts;
using AddieSoft.Membership.Repositories;
using DevSkill.Data;

namespace AddieSoft.Membership.UnitOfWorks
{
    public class MembershipUnitOfWork : UnitOfWork, IMembershipUnitOfWork
    {
        public IUserInfoRepository UserInfoRepository { get; private set; }

        public MembershipUnitOfWork(IMembershipDbContext dbContext, 
            IUserInfoRepository userInfoRepository)
            : base((MembershipDbContext)dbContext)
        {
            UserInfoRepository = userInfoRepository;
        }
    }
}