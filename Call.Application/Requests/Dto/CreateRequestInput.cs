using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Call.Calls;
using Call.Users;

namespace Call.Requests.Dto
{
    [AutoMap(typeof(Request))]
    public class CreateRequestInput
    {
        [Required]
        public int TenantId { get; set; }
        [Required]
        [Phone]
        [StringLength(User.MaxPhoneLength)]
        public string Phone { get; set; }
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string Email { get; set; }

        public DateTime Created { get; set; }
        public State State { get; set; }
        [MaxLength(1024)]
        public string Question { get; set; }
    }
}
