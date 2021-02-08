using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AcademicPortalApp.Models
{
    public class CourseAndCategoryViewModel
    {
        public Courses Course { get; set; }
        public Categories Category { get; set; }
        public IEnumerable<Categories> Categories { get; set; }

    }
    public class TrainerInStaffViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public int TypeId { get; set; }
        public string WorkingPlace { get; set; }
        public string TrainerName { get; set; }
        public IEnumerable<Types> Types { get; set; }
        public Types Type { get; set; }
        public string Id { get; set; }
    }
}