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
          

            var checkIfExist = _context.TrainerCourses.SingleOrDefault(t => t.CourseId == model.CourseId && t.TrainerId == model.TrainerId);
            if (checkIfExist != null)
            {
                var viewModel = new ViewModelCoursesTrainer()
                {
                    Trainers = _context.Users.OfType<Trainer>().ToList(),
                    Courses = _context.Courses.ToList()
                };
                ViewBag.message = "This courses had been assigned to this trainer";
                return View(viewModel);
            }
            else
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
        }


        //GET: Staff/ find trainer course by id and trainer id and create a new view model of trainer course to return trainer course, list of course and trainer id
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignedTrainerCourse(int Id)
        {
            var trainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == Id);
            var trainerId = trainerCourse.TrainerId;
            ViewModelCoursesTrainer model = new ViewModelCoursesTrainer
            {
                TrainerCourse = trainerCourse,
                Courses = _context.Courses.ToList(),
                TrainerId = trainerId
            };

            return View(model);
        }
        //POST: Staff/ find trainer course by trainer course by id and change courseid that receive from view model
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult ReassignedTrainerCourse(ViewModelCoursesTrainer model)
        {
            var trainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == model.TrainerCourse.Id);
            trainerCourse.CourseId = model.TrainerCourse.CourseId;
            _context.SaveChanges();

           return RedirectToAction("AllCourseRelatedTrainer", "TrainerCourses", new { trainerId = model.TrainerId });
        }





     //Remove : Staff remove trainer

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTrainerCourse(int Id)
        {
            var findTrainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == Id);
            var trainerId = findTrainerCourse.TrainerId;
            _context.TrainerCourses.Remove(findTrainerCourse);
            _context.SaveChanges();
            return RedirectToAction("AllCourseRelatedTrainer", "TrainerCourses", new { trainerId = trainerId });
        }
















        



    }
}