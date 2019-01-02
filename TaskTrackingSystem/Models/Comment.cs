using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [Required]
        public DateTime PostTime { get; set; }

        [Required]
        public string Content { get; set; }

        public virtual Work Work { get; set; }
        public ApplicationUser User { get; set; }
    }
}
