using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using Microsoft.Owin.Security;
using AcademicPortalApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AcademicPortalApp.Controllers
{
    [Authorize(Roles ="Staff")]
    public class StaffController : Controller
    {
        private ApplicationUserManager _userManager;
        private UserManager<ApplicationUser> _manager;
        private ApplicationSignInManager _signInManager;
        private ApplicationDbContext _context;
        private ApplicationUser _user;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public StaffController()

        {
            _context = new ApplicationDbContext();
            _user = new ApplicationUser();
            _manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public StaffController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;


        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



       
        //GET: /Staff/Create Course
        [Authorize(Roles ="Staff")]
        public ActionResult CreateCourse()
        {
           var selectedcategorylist = new CourseAndCategoryViewModel()
            {
                Categories = _context.Categories.ToList()
            };
            return View(selectedcategorylist);
        }
        //POST: /Staff/Create Course
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(CourseAndCategoryViewModel model)
        {
            _context.Courses.Add(model.Course);
            _context.SaveChanges();

            return RedirectToAction("AllCourse");
        }
        //GET: /Staff/Edit Course
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditCourse(int Id)
        {
            var modelInfo = new CourseAndCategoryViewModel()
            {
                Course = _context.Courses.SingleOrDefault(t => t.Id == Id),
                Categories = _context.Categories.ToList()
            };
            return View(modelInfo);
        }
        //POST: /Staff/Edit Course
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult EditCourse(CourseAndCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var modelInfo = new CourseAndCategoryViewModel()
                {
                    Course = model.Course,
                    Categories = _context.Categories.ToList()
                };
                return View(modelInfo);
            }       
            var findCourse = _context.Courses.SingleOrDefault(c => c.Id == model.Course.Id);
            if (findCourse == null) 
            {
                return HttpNotFound();
            }
            findCourse.Name = model.Course.Name;
            findCourse.Description = model.Course.Description;
            findCourse.CategoryId = model.Course.CategoryId;
            _context.SaveChanges();
            return RedirectToAction("AllCourse");
        }
        // Staff/Delete Course
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteCourse(int Id)
        {
            var findCourse = _context.Courses.SingleOrDefault(c => c.Id == Id);
            _context.Courses.Remove(findCourse);
            _context.SaveChanges();
            return RedirectToAction("AllCourse");
        }
        // get all category and search by category name
        [Authorize(Roles = "Staff")]
        public ActionResult AllCategory(string cateName)
        {
            if (String.IsNullOrWhiteSpace(cateName))
            {
                return View(_context.Categories.ToList());
            }
            return View(_context.Categories.Where(t => t.Name.Contains(cateName)).ToList());
        }
        // GET: /Staff/Create Category
        [Authorize(Roles ="Staff")]
        public ActionResult CreateCategory()
        { 
            return View();
        }
        //POST: /Staff/Create Category
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateCategory(Categories c)
        {
            _context.Categories.Add(c);
            _context.SaveChanges();
            return RedirectToAction("AllCategory");
        }
        //GET: /Staff/Edit Category
        [Authorize(Roles ="Staff")]
        public ActionResult EditCategory(int Id)
        {
            var cate = _context.Categories.SingleOrDefault(t => t.Id == Id);
            return View(cate);
        }
        //POST: /Staff/Edit Category
        [HttpPost]
        [Authorize(Roles ="Staff")]
        public ActionResult EditCategory(Categories c)
        {
            var cateInfo = _context.Categories.SingleOrDefault(t => t.Id == c.Id);
            cateInfo.Name = c.Name;
            cateInfo.Description = c.Description;
            _context.SaveChanges();
            return RedirectToAction("AllCategory");
        }
        // Staff/Delete Category
        [Authorize(Roles ="Staff")]
        public ActionResult DeleteCategory(int Id)
        {
            var findCate = _context.Categories.SingleOrDefault(t => t.Id == Id);
            _context.Categories.Remove(findCate);
            _context.SaveChanges();
            return RedirectToAction("AllCategory");
        }


        // Staff /get all Trainer
        public ActionResult AllTrainer()
        {
            var allTrainer = _context.Users.OfType<Trainer>().Include(t => t.Type).ToList();

            List<TrainerViewModel> trainerInfo = new List<TrainerViewModel>();

            foreach (var trainer in allTrainer)
            {

                trainerInfo.Add(new TrainerViewModel()
                {
                    Trainer = trainer,
                    UserName = trainer.UserName,
                    Id = trainer.Id
                });
            }

            return View(trainerInfo);
        }


        //Get : Staff/Edit Trainer
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainer(string Id)
        {
            var findTrainer = _context.Users.OfType<Trainer>().Include(t => t.Type).SingleOrDefault(t => t.Id == Id);
            if (findTrainer == null)
            {
                return HttpNotFound();
            }
            TrainerInStaffViewModel model = new TrainerInStaffViewModel()
            {
                Id = findTrainer.Id,
                TrainerName = findTrainer.TrainerName,
                Email = findTrainer.Email,
                WorkingPlace = findTrainer.WorkingPlace,
                Type = findTrainer.Type,
                Types = _context.Types.ToList()
            };
            return View(model);
        }
        //POST: /Staff/Edit Trainer
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrainer(TrainerInStaffViewModel model)
        {

            var findTrainer = _context.Users.OfType<Trainer>().SingleOrDefault(t => t.Id == model.Id);
            findTrainer.TrainerName = model.TrainerName;
            findTrainer.TypeId = model.TypeId;
            findTrainer.WorkingPlace = model.WorkingPlace;
            _context.SaveChanges();
            return RedirectToAction("AllTrainer");
        }
        //get all trainee and search trainee infomation
        [Authorize(Roles = "Staff")]
        public ActionResult AllTrainee(string traineeName)
        {
            var allTrainee = _context.Users.OfType<Trainee>().Include(t => t.ProgrammingLanguage).ToList();

            List<TraineeInStaffViewModel> traineeInfo = new List<TraineeInStaffViewModel>();

            if (String.IsNullOrWhiteSpace(traineeName))
            {

                foreach (var trainee in allTrainee)
                {

                    traineeInfo.Add(new TraineeInStaffViewModel()
                    {
                        TraineeName = trainee.TraineeName,
                        Email = trainee.UserName,
                        Id = trainee.Id,
                        Age = trainee.Age,
                        ProgrammingLanguage = trainee.ProgrammingLanguage,
                        ProgrammingLanguageId = trainee.ProgrammingLanguageId,
                        TOEICScore = trainee.TOEICScore,
                        ExperienceDetails = trainee.ExperienceDetails,
                        DateOfBirth = trainee.DateOfBirth,
                        Department = trainee.Department,
                        Location = trainee.Location
                    });
                }
                return View(traineeInfo);
            }
            else
            {
                allTrainee = _context.Users.OfType<Trainee>()
                    .Where(trainee => trainee.TraineeName == traineeName)
                    .Include(t => t.ProgrammingLanguage).ToList();

                foreach (var trainee in allTrainee)
                {
                    traineeInfo.Add(new TraineeInStaffViewModel()
                    {
                        TraineeName = trainee.TraineeName,
                        Email = trainee.UserName,
                        Id = trainee.Id,
                        Age = trainee.Age,
                        ProgrammingLanguage = trainee.ProgrammingLanguage,
                        ProgrammingLanguageId = trainee.ProgrammingLanguageId,
                        TOEICScore = trainee.TOEICScore,
                        ExperienceDetails = trainee.ExperienceDetails,
                        DateOfBirth = trainee.DateOfBirth,
                        Department = trainee.Department,
                        Location = trainee.Location
                    });
                }
                return View(traineeInfo);
            }

        }
        //Get Staff /Create : Trainee
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult CreateTrainee()
        {
            TraineeInStaffViewModel model = new TraineeInStaffViewModel()
            {
                ProgrammingLanguages = _context.ProgrammingLanguages.ToList()
            };
            return View(model);
        }

        
        // POST: /Staff/Create new trainee account
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainee(TraineeInStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Trainee { UserName = model.Email, Email = model.Email, DateOfBirth = model.DateOfBirth, ProgrammingLanguageId = model.ProgrammingLanguageId, TraineeName = model.TraineeName, Age = model.Age, Department = model.Department, Location = model.Location, ExperienceDetails = model.ExperienceDetails };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var userRole = UserManager.AddToRole(user.Id, "Trainee");
                    return RedirectToAction("AllTrainee");
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET : Staff /Edit Trainee
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainee(string Id)
        {
            var findTrainee = _context.Users.OfType<Trainee>().Include(t => t.ProgrammingLanguage).SingleOrDefault(t => t.Id == Id);
            if (findTrainee == null)
            {
                return HttpNotFound();
            }
            TraineeInStaffViewModel model = new TraineeInStaffViewModel()
            {
                Id = findTrainee.Id,
                TraineeName = findTrainee.TraineeName,
                Email = findTrainee.Email,
                Age = findTrainee.Age,
                DateOfBirth = findTrainee.DateOfBirth,
                TOEICScore = findTrainee.TOEICScore,
                ExperienceDetails = findTrainee.ExperienceDetails,
                Department = findTrainee.Department,
                Location = findTrainee.Location,
                ProgrammingLanguages = _context.ProgrammingLanguages.ToList()
            };
            return View(model);
        }

        //Post : Staff / Edit Trainee
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrainee(TraineeInStaffViewModel model)
        {
            var findTrainee = _context.Users.OfType<Trainee>().SingleOrDefault(t => t.Id == model.Id);
            findTrainee.TraineeName = model.TraineeName;
            findTrainee.Email = model.Email;
            findTrainee.Age = model.Age;
            findTrainee.DateOfBirth = model.DateOfBirth;
            findTrainee.TOEICScore = model.TOEICScore;
            findTrainee.ExperienceDetails = model.ExperienceDetails;
            findTrainee.Department = model.Department;
            findTrainee.Location = model.Location;
            findTrainee.ProgrammingLanguageId = model.ProgrammingLanguageId;
            _context.SaveChanges();
            return RedirectToAction("AllTrainee");
        }


        //Remove : Staff / Delete Trainee
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteTrainee(string Id)
        {
            var findTrainee = _context.Users.SingleOrDefault(t => t.Id == Id);
            if (findTrainee == null)
            {
                return HttpNotFound();
            }
            _context.TraineeCourses.RemoveRange(_context.TraineeCourses.Where(t => t.TraineeId == Id));
            _context.Users.Remove(findTrainee);
            _context.SaveChanges();
            return RedirectToAction("AllTrainee");
        }

        // get all course and search course by name
        public ActionResult AllCourse(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return View(_context.Courses.Include(t => t.Category).ToList());
            }
            return View(_context.Courses.Where(t => t.Name.Contains(name)).Include(t => t.Category).ToList());
        }


    }
}