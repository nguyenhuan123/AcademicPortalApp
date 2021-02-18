using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcademicPortalApp.Models
{
    public class Trainee : ApplicationUser
    {
        [Required(ErrorMessage = "Trainee name must not be empty")]
        public string TraineeName { get; set; }
        [Required(ErrorMessage = "Age must not be empty")]
        public string Age { get; set; }
        [Required(ErrorMessage = "Date of birth must not be empty")]
        public string DateOfBirth { get; set; }
        public int TOEICScore { get; set; }
        public string ExperienceDetails { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public ProgrammingLanguages ProgrammingLanguage { get; set; }
        [Required(ErrorMessage = " Programming Language must not be empty")]
        public int ProgrammingLanguageId { get; set; }

    }
}