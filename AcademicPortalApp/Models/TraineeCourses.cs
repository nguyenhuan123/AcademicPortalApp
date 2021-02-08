using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AcademicPortalApp.Models
{
    public class TraineeCourses
    {
        [Key]
        public int Id { get; set; }
        public Courses Course { get; set; }
        public int CourseId { get; set; }
        public Trainee Trainee { get; set; }
        public string TraineeId { get; set; }
    }
}