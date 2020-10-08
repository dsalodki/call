using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Call.Calls;

namespace Call.Requests.Dto
{
    [AutoMapFrom(typeof(Request))]
    public class RequestListDto : EntityDto<long>
    {
        //public int TenantId { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public TimeSpan Treated { get; set; }

        public TimeSpan Answered { get; set; }

        public State State { get; set; }

        [MaxLength(1024)]
        public string Question { get; set; }

        public long? UserId { get; set; }

    }
}
