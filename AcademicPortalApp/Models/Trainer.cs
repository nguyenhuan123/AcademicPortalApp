using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademicPortalApp.Models
{
    public class Trainer : ApplicationUser
    {
        public string TrainerName { get; set; }
        public int TypeId { get; set; }
        public Types Type { get; set; }
        public string WorkingPlace { get; set; }

    }
}