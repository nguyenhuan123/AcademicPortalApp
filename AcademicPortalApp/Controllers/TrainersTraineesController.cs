using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicPortalApp.Models;


namespace AcademicPortalApp.Controllers
{
    [Authorize]
    public class TrainersTraineesController : Controller
    {
        private ApplicationDbContext _context;

        public TrainersTraineesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Users (Trainer/Trainee) related courses
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userCourse = new List<TrainerTranineeViewModel >();
            if (User.IsInRole("Trainer"))
            {
                var trainerCourses = _context.TrainerCourses
                    .Where(t => t.TrainerId == userId)
                    .Include(t => t.Trainer)
                    .Include(t => t.Course).ToList();

                foreach (var trainerCourse in trainerCourses)
                {
                    userCourse.Add(new TrainerTranineeViewModel()
                    {
                        TrainerCourse = trainerCourse
                    });
                }
                return View(userCourse);
            }
            else if (User.IsInRole("Trainee"))
            {
                var traineeCourses = _context.TraineeCourses
                    .Where(t => t.TraineeId == userId)
                    .Include(t => t.Trainee)
                    .Include(t => t.Course).ToList();
                foreach (var traineeCourse in traineeCourses)
                {
                    userCourse.Add(new TrainerTranineeViewModel()
                    {
                        TraineeCourse = traineeCourse
                    });
                }
                return View(userCourse);
            }
            return View(userCourse);
        }
        [Authorize(Roles = "Trainee")]
        public ActionResult ViewAllCourse()
        {
            return View(_context.Courses.Include(t => t.Category).ToList());
        }
    }
}