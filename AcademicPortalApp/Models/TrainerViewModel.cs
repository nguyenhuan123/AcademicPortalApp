using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademicPortalApp.Models
{
    public class TrainerViewModel
    {
        public string Id { get; set; }
        public Trainer Trainer { get; set; }
        public string UserName { get; set; }
    }
}