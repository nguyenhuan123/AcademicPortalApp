using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicPortalApp.Models;

namespace AcademicPortalApp.Controllers
{
    public class TrainerCoursesController : Controller
    {
        // GET: TrainerCoures
        private ApplicationDbContext _context;

        public TrainerCoursesController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: TrainerCourses
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllCourseRelatedTrainer(string trainerId)
        {
            var allCoursesRelatedTrainer= _context.TrainerCourses
                .Where(t => t.TrainerId == trainerId).Include(t => t.Trainer).Include(t => t.Course).ToList();
            return View(allCoursesRelatedTrainer);
        }


        //GET: Staff/Assign Course to trainer: return :select trainer and course view model to view 
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCoursesTrainers()
        {
            var viewModel = new ViewModelCoursesTrainer()
            {
                Trainers = _context.Users.OfType<Trainer>().ToList(),
                Courses = _context.Courses.ToList()
            };

            return View(viewModel);
        }
        //POST: Staff/ receive trainerid and courseid from viewmodel to create new trainer course and redirect to all trainer course page 
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCoursesTrainers(ViewModelCoursesTrainer model)
        {
            var trainerCourse = new TrainerCourses()
            {
                TrainerId = model.TrainerId,
                CourseId = model.CourseId
            };

            _context.TrainerCourses.Add(trainerCourse);
            _context.SaveChanges();

            return RedirectToAction("AllCourseRelatedTrainer", "TrainerCourses", new { trainerId = model.TrainerId });
        }







     

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTrainerFromCourse(int Id)
        {
            var findTrainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == Id);
            var trainerId = findTrainerCourse.TrainerId;
            _context.TrainerCourses.Remove(findTrainerCourse);
            _context.SaveChanges();
            return RedirectToAction("AllCourseRelatedTrainer", "TrainerCourses", new { trainerId = trainerId });
        }
















        // find trainer course by id and assign trainerid for redirect to all trainer course then remove trainer course had been found



    }
}