using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicPortalApp.Models;

namespace AcademicPortalApp.Controllers
{
    public class TrainerCouresController : Controller
    {
        // GET: TrainerCoures
        private ApplicationDbContext _context;

        public TrainerCouresController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: TrainerRelated
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AllCourseRelatedTrainer(string trainerId)
        {
            var allCoursesRelatedTrainer= _context.TrainerCourses
                .Where(t => t.TrainerId == trainerId).Include(t => t.Trainer).Include(t => t.Course).ToList();
            return View(allCoursesRelatedTrainer);
        }








        // find trainer course by id and assign trainerid for redirect to all trainer course then remove trainer course had been found
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTrainerFromCourse(int Id)
        {
            var findTrainerCourse = _context.TrainerCourses.SingleOrDefault(t => t.Id == Id);
            var trainerId = findTrainerCourse.TrainerId;
            _context.TrainerCourses.Remove(findTrainerCourse);
            _context.SaveChanges();
            return RedirectToAction("AllCourseOfTrainer", "TrainerRelated", new { trainerId = trainerId });
        }


    }
}