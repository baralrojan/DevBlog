using System.ComponentModel.DataAnnotations;

namespace DevBlog.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Name field must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The DisplayName field is required.")]
        [StringLength(50, ErrorMessage = "The DisplayName field cannot exceed 50 characters.")]
        public string DisplayName { get; set; }
    }
}
