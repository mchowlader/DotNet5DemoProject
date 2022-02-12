using AddieSoft.Foundation.Services;
using AddieSoft.Membership.BusinessObjects;
using AddieSoft.Membership.Services;
using Autofac;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
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
        private IFileStoreUtility _fileStoreUtility;
        private ISystemImageResizer _systemImageResizer;


        public CreateModel()
        {
        }

        public CreateModel(IUserService service, 
            IFileStoreUtility fileStoreUtility,
            ISystemImageResizer systemImageResizer)
        {
            _service = service;
            _fileStoreUtility = fileStoreUtility;
            _systemImageResizer = systemImageResizer;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _service = _scope.Resolve<IUserService>();
            _fileStoreUtility = _scope.Resolve<IFileStoreUtility>();
            _systemImageResizer = _scope.Resolve<ISystemImageResizer>();
        }

        public async Task UserCreateAsync()
        {
            if(FromFile != null)
            {
                var tempImage = new FileInfo(_fileStoreUtility.StoreFile(FromFile).filePath);
                var resizeImage = await _systemImageResizer.ProfileImageResizeAsync(tempImage);
                Photo = resizeImage.Name;
            }
            else
            {
                if(Photo != null)
                    Photo = null;
            }
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