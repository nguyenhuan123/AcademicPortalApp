using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcademicPortalApp.Models
{
    public class Types
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Type name must not be empty")]
        public string Name { get; set; }
    }
}