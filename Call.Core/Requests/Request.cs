using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Call.MultiTenancy;
using Call.Users;

namespace Call.Calls
{
    public class Request : Entity<long>
    {
        [Required]
        [ForeignKey("Tenant")]
        public int TenantId { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public DateTime Created { get; set; }
        public TimeSpan TreatedTime { get; set; }
        public TimeSpan AnsweredTime { get; set; }
        public State State { get; set; }
        [MaxLength(1024)]
        public string Question { get; set; }
        [ForeignKey("User")]
        public long? UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
