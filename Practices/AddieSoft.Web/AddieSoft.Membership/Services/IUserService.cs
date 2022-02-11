using AddieSoft.Membership.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddieSoft.Membership.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(UserInfo user);
        Task<(IList<UserInfo> records, int total, int totalDispaly )> GetUserDataAsync
            (int pageIndex, int pageSize, string searchText, string sortText);
        Task<UserInfo> LoadDataAsync(int id);
        Task UpdateUserAsync(UserInfo user);
        Task DeleteAsync(int id);
    }
}
