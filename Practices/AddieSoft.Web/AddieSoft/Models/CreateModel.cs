using AddieSoft.Membership.BusinessObjects;
using AddieSoft.Membership.Services;
using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AddieSoft.Models
{
    public class CreateModel
    {

        public string Name { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public IFormFile FromFile { get; set; }


        private ILifetimeScope _scope;
        private IUserService _service;

        public CreateModel()
        {
        }

        public CreateModel(IUserService service)
        {
            _service = service;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _service = _scope.Resolve<IUserService>();
        }

        public async Task UserCreateAsync()
        {
            var user = new UserInfo()
            {
                Name = Name,
                Address = Address,
                MobileNumber = MobileNumber,
                CreatedDate = DateTime.Now,
                Gender = Gender,
                Photo = Photo
            };

            await _service.CreateUserAsync(user);
        }
    }
}