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
    public class TraineeInStaffViewModel
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
        public string TraineeName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public int TOEICScore { get; set; }
        public string ExperienceDetails { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public ProgrammingLanguages ProgrammingLanguage { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public IEnumerable<ProgrammingLanguages> ProgrammingLanguages { get; set; }
        public string Id { get; set; }
    }
    public class ViewModelCoursesTrainer
    {
        public string TrainerId { get; set; }
        public IEnumerable<Trainer> Trainers { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Courses> Courses { get; set; }
        public TrainerCourses TrainerCourse { get; set; }
    }

    public class ViewModelCoursesTrainee
    {
        public string TraineeId { get; set; }
        public IEnumerable<Trainee> Trainees { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Courses> Courses { get; set; }
        public TraineeCourses TraineeCourse { get; set; }
    }

}
