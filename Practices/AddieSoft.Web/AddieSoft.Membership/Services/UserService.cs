using AddieSoft.Membership.BusinessObjects;
using AddieSoft.Membership.UnitOfWorks;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddieSoft.Membership.Services
{
    public class UserService : IUserService
    {
        private IMembershipUnitOfWork _unitOfWork;
       //private readonly IMapper _mapper;

        public UserService(IMembershipUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task CreateUserAsync(UserInfo user)
        {
            await _unitOfWork.UserInfoRepository.AddAsync(
               new Entities.UserInfo()
               { 
                    Address = user.Address,
                    CreatedDate = user.CreatedDate,
                    Gender = user.Gender,
                    Name = user.Name,
                    MobileNumber = user.MobileNumber,
                    Photo = user.Photo,
               });

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
           await _unitOfWork.UserInfoRepository.RemoveAsync(id);
           await _unitOfWork.SaveAsync();
        }

        public async Task<(IList<UserInfo> records, int total, int totalDispaly)> GetUserDataAsync
            (int pageIndex, int pageSize, string searchText, string sortText)
        {
            var userList = await _unitOfWork.UserInfoRepository.GetDynamicAsync(
                string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
                sortText, null, pageIndex, pageSize, false);

            var result = (from user in userList.data
                          select new UserInfo()
                          {
                              Name = user.Name,
                              Address = user.Address,
                              CreatedDate = user.CreatedDate,
                              Gender = user.Gender,
                              MobileNumber = user.MobileNumber,
                              Id = user.Id
                          }).ToList();

            return (result, userList.total, userList.totalDisplay);
        }

        public async Task<UserInfo> LoadDataAsync(int id)
        {
            var student = await _unitOfWork.UserInfoRepository.GetByIdAsync(id);
            return new UserInfo()
            {
                Name = student.Name,
                Address = student.Address,
                CreatedDate = student.CreatedDate,
                MobileNumber = student.MobileNumber,
                Gender = student.Gender,
            };
        }

        public async Task UpdateUserAsync(UserInfo user)
        {
            var userEntity = await _unitOfWork.UserInfoRepository.GetByIdAsync(user.Id);

            if (userEntity != null)
            {
                userEntity.Id = user.Id;
                userEntity.Name = user.Name;
                userEntity.Address = user.Address;
                userEntity.CreatedDate = DateTime.Now;
                userEntity.Gender = user.Gender;
                userEntity.MobileNumber = user.MobileNumber;

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
