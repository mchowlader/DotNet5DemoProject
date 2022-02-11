using AddieSoft.Membership.Repositories;
using DevSkill.Data;

namespace AddieSoft.Membership.UnitOfWorks
{
    public interface IMembershipUnitOfWork : IUnitOfWork
    {
        public IUserInfoRepository UserInfoRepository { get; }
    }
}