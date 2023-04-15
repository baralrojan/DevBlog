using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevBlog.Library.Models.ViewModels
{
    public class AddBlogPostRequest
    {

        [Required(ErrorMessage = "The Heading field is required.")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "The Page Title field is required.")]
        public string PageTitle { get; set; }

        [Required(ErrorMessage = "The Content field is required.")]
        [StringLength(5, MinimumLength = 10, ErrorMessage = "The content field must be between 10 and 50 characters long.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The Short Description field is required.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The Short Description field must be between 10 and 200 characters long.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "The Featured Image URL field is required.")]
        [Url(ErrorMessage = "The Featured Image URL field must be a valid URL.")]
        public string FeaturedImageUrl { get; set; }

       //[Required(ErrorMessage = "The URL Handle field is required.")]
      //  [RegularExpression("^[a-zA-Z0-9_-]*$", ErrorMessage = "The URL Handle field can only contain letters, numbers, dashes, and underscores.")]
        public string UrlHandle { get; set; }

        [Required(ErrorMessage = "The Published Date field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The Published Date field must be a valid date.")]
        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "The Author field is required.")]
        public string Author { get; set; }

        public bool Visible { get; set; }

        // Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
