using AddieSoft.Membership.Services;
using Autofac;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AddieSoft.Models
{
    public class DataModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }

        private ILifetimeScope _scope;
        private IUserService _service;

        public DataModel()
        {
        }

        public DataModel(IUserService service)
        {
            _service = service;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _service = _scope.Resolve<IUserService>();
        }

        public async Task<object> GetUserDataAsyns(DataTablesAjaxRequestModel dataTableModel)
        {
            var data = await _service.GetUserDataAsync(
                dataTableModel.PageIndex,
                dataTableModel.PageSize,
                dataTableModel.SearchText,
                dataTableModel.GetSortText(new string[] {"Photo", "Name", "Address", "Gender", "MobileNumber", "CreatedDate" }));
            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDispaly,
                data = (from record in data.records
                            select new string[]
                            {
                                record.Photo,
                                record.Name,
                                record.Address,
                                record.Gender,
                                record.MobileNumber.ToString(),
                                record.CreatedDate.ToString(),
                                record.Id.ToString()
                            }
                        ).ToArray()
            };
        }

        public async Task DeleteDataAsync(int id)
        {
           await _service.DeleteAsync(id);
        }
    }
}