using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaprosBy.Feedback.Api.Models
{
    public class SubmitFormModel
    {
        [Phone]
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ProviderId { get; set; }
    }
}
