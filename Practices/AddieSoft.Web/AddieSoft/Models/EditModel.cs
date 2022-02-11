using AddieSoft.Membership.BusinessObjects;
using AddieSoft.Membership.Services;
using Autofac;
using System;
using System.Threading.Tasks;

namespace AddieSoft.Models
{
    public class EditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }

        private ILifetimeScope _scope;
        private IUserService _service;

        public EditModel()
        {
        }

        public EditModel(IUserService service)
        {
            _service = service;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _service = _scope.Resolve<IUserService>();
        }

        internal async Task LoadDataAsync(int Id)
        {
            var data = await _service.LoadDataAsync(Id);

            if (data != null)
            {
                Name = data.Name;
                Address = data.Address;
                MobileNumber = data.MobileNumber;
                CreatedDate = data.CreatedDate;
                Gender = data.Gender;
            }
        }

        internal async Task UpadteAsync()
        {
            var user = new UserInfo()
            {
                Id = Id,
                Name = Name,
                Address = Address,
                MobileNumber = MobileNumber,
                CreatedDate = DateTime.Now,
                Gender = Gender,
                Photo = Photo
            };

            await _service.UpdateUserAsync(user);
        }
    }
}