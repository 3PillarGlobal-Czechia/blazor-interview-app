using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InterviewApp.Shared.Models
{
    public class InterviewQuestion
    {
        public string? Title { get; set; }

        [Required]
        public int? Difficulty { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? Content { get; set; }

        public int Rating { get; set; }

        public bool? IsPinned { get; set; }

        public string? Note { get; set; }

        public bool Contains(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(Title))
                Title = string.Empty;

            if (string.IsNullOrWhiteSpace(Content))
                Content = string.Empty;

            if (string.IsNullOrWhiteSpace(Category))
                Category = string.Empty;

            if (string.IsNullOrWhiteSpace(Note))
                Note = string.Empty;

            value = value.Trim().ToLower();

            return Title.ToLower().Contains(value) ||
                   Content.ToLower().Contains(value) || 
                   Category.ToLower().Contains(value) ||
                   Note.ToLower().Contains(value);
        }

        public InterviewQuestion Reset(int index)
        {
            Title = $"Question {index + 1}";
            IsPinned = false;
            Note = null;
            Rating = 0;

            return this;
        }

        public InterviewQuestion Reset(string title)
        {
            Title = title;
            IsPinned = false;
            Note = null;
            Rating = 0;

            return this;
        }

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
