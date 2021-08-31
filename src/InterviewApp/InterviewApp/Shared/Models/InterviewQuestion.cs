using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.Shared.Models
{
    public class InterviewQuestion
    {
        [Required]
        public int Difficulty { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? Content { get; set; }

        public string? Rating { get; set; }
    }
}
