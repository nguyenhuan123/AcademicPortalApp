using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademicPortalApp.Models
{
    public class Trainee : ApplicationUser
    {
        public string TraineeName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public int TOEICScore { get; set; }
        public string ExperienceDetails { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public ProgrammingLanguages ProgrammingLanguage { get; set; }
        public int ProgrammingLanguageId { get; set; }

    }
}