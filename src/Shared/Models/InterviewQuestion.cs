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
        public string? Title { get; set; }

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? Content { get; set; }

        public int Rating { get; set; }

        public bool IsPinned { get; set; }

        public string? Note { get; set; }

        public override string ToString()
        {
            var text = new StringBuilder();

            text.AppendLine(Title);
            text.AppendLine($"Rating: {Rating}; Difficulty: {Difficulty}");
            text.AppendLine(Note);

            return text.ToString();
        }
    }
}
