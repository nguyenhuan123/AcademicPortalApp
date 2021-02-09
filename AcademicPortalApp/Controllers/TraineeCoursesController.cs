using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicPortalApp.Models;

namespace AcademicPortalApp.Controllers
{
    //Get TraineeCourses
    public class TraineeCoursesController : Controller
    {
        private ApplicationDbContext _context;

        public TraineeCoursesController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: TraineeCourses

        // GET: TrainerCourses
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllCourseRelatedTrainee(string traineeId)
        {
            var allCoursesRelatedTrainee = _context.TraineeCourses
                .Where(t => t.TraineeId == traineeId).Include(t => t.Trainee).Include(t => t.Course).ToList();
            return View(allCoursesRelatedTrainee);
        }

    }
}