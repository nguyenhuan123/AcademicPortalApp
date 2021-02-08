using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace AcademicPortalApp.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Category must be named")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Category Description must not be empty")]
        public string Description { get; set; }
    }
}