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


        // GET: TraineeCourse
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllCourseRelatedTrainee(string traineeId)
        {
            var allCoursesRelatedTrainee = _context.TraineeCourses
                .Where(t => t.TraineeId == traineeId).Include(t => t.Trainee).Include(t => t.Course).ToList();
            return View(allCoursesRelatedTrainee);
        }


        //GET: Staff/Assign Course to trainee: return :select trainee and course view model to view 
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCoursesTrainees()
        {
            var viewModel = new ViewModelCoursesTrainee()
            {
                Trainees = _context.Users.OfType<Trainee>().ToList(),
                Courses = _context.Courses.ToList()
            };

            return View(viewModel);
        }
        //POST: Staff/ receive traineeid and courseid from viewmodel to create new trainee course and redirect to all trainer course page 
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCoursesTrainees(ViewModelCoursesTrainee model)
        {
            var traineeCourse = new TraineeCourses()
            {
                TraineeId = model.TraineeId,
                CourseId = model.CourseId
            };

            _context.TraineeCourses.Add(traineeCourse);
            _context.SaveChanges();

            return RedirectToAction("AllCourseRelatedTrainee", "TraineeCourses", new { traineeId = model.TraineeId });
        }


        //Remove :Staff remove trainee
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTraineeCourse(int Id)
        {
            var findTraineeCourse = _context.TraineeCourses.SingleOrDefault(t => t.Id == Id);
            var traineeId = findTraineeCourse.TraineeId;
            _context.TraineeCourses.Remove(findTraineeCourse);
            _context.SaveChanges();
            return RedirectToAction("AllCourseRelatedTrainee", "TraineeCourses", new { traineeId = traineeId });
        }
    }
}