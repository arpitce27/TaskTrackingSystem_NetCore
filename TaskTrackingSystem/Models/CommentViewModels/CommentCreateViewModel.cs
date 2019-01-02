using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.Models.CommentViewModels
{
    public class CommentCreateViewModel
    {
        public int Id { get; set; }

        [Requried]
        [Display(Name = "Comment")]
        public string Content { get; set; }
    }
}
