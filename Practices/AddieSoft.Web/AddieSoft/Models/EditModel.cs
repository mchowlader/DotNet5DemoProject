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
    public class EditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public IFormFile FormFile { get; set; }

        private ILifetimeScope _scope;
        private IUserService _service;
        private IPathService _pathService;
        private IFileStoreUtility _fileStoreUtility;
        private ISystemImageResizer _systemImageResizer;

        public EditModel()
        {
        }

        public EditModel(IUserService service, 
            IPathService pathService, 
            IFileStoreUtility fileStoreUtility,
            ISystemImageResizer systemImageResizer)
        {
            _service = service;
            _pathService = pathService;
            _fileStoreUtility = fileStoreUtility;
            _systemImageResizer = systemImageResizer;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _service = _scope.Resolve<IUserService>();
            _pathService = _scope.Resolve<IPathService>();
            _fileStoreUtility = _scope.Resolve<IFileStoreUtility>();
            _systemImageResizer = _scope.Resolve<ISystemImageResizer>();
        }

        internal async Task LoadDataAsync(int Id)
        {
            var data = await _service.LoadDataAsync(Id);

            if (data != null)
            {
                if(data.Photo == null)
                {
                    var defaultProfileImage = _pathService.DefaultProfileImage;
                    Photo = _pathService.AttachPathWithDefaultProfileImage(defaultProfileImage);
                }
                else
                {
                    Photo = _pathService.AttachPathWithFile(data.Photo);
                }

                Name = data.Name;
                Address = data.Address;
                MobileNumber = data.MobileNumber;
                CreatedDate = data.CreatedDate;
                Gender = data.Gender;
            }
        }

        internal async Task UpadteAsync()
        {
            if(FormFile != null)
            {
                var temporaryImage = new FileInfo(_fileStoreUtility.StoreFile(FormFile).filePath);
                var resizeImage = await _systemImageResizer.ProfileImageResizeAsync(temporaryImage);
                Photo = resizeImage.Name;
            }
            else
            {
                Photo= Photo.Remove(0,7);
            }

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