using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddieSoft.Membership.Entities
{
    public class UserInfo : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
    }
}
