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
            var checkIfExist = _context.TraineeCourses.SingleOrDefault(t => t.TraineeId == model.TraineeId && t.CourseId == model.CourseId);
            if(checkIfExist != null)
            {
                var viewModel = new ViewModelCoursesTrainee()
                {
                    Trainees = _context.Users.OfType<Trainee>().ToList(),
                    Courses = _context.Courses.ToList()
                };
                ViewBag.message = "This courses had been assigned to this trainee"; 
                return View(viewModel);
            }
            else
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


        //GET: Staff/ find trainee course by id and trainee id and create a new view model of trainee course to return trainee course, list of course and trainee id
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignedTraineeCourse(int Id)
        {
            var traineeCourse = _context.TraineeCourses.SingleOrDefault(t => t.Id == Id);
            var traineeId = traineeCourse.TraineeId;
            ViewModelCoursesTrainee model = new ViewModelCoursesTrainee
            {
                TraineeCourse = traineeCourse,
                Courses = _context.Courses.ToList(),
                TraineeId = traineeId
            };

            return View(model);
        }
        //POST: Staff/ find trainee course by trainee course by id and change courseid that receive from view model
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignedTraineeCourse(ViewModelCoursesTrainee model)
        {
            var traineeCourse = _context.TraineeCourses.SingleOrDefault(t => t.Id == model.TraineeCourse.Id);
            traineeCourse.CourseId = model.TraineeCourse.CourseId;
            _context.SaveChanges();

            return RedirectToAction("AllCourseRelatedTrainee", "TraineeCourses", new { traineeId = model.TraineeId });
        }


    }
}