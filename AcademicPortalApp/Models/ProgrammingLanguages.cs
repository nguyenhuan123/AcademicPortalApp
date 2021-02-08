using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcademicPortalApp.Models
{
    public class ProgrammingLanguages
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name must not be empty")]
        public string Name { get; set; }
    }
}