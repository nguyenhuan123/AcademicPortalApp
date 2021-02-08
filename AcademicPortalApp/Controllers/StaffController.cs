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
        // get all course
        public ActionResult AllCourse()
        {
            var allCourse = _context.Courses.ToList();

            List<CourseAndCategoryViewModel> courseWithCate = new List<CourseAndCategoryViewModel>();

            foreach(var course in allCourse)
            {
                courseWithCate.Add(new CourseAndCategoryViewModel()
                {
                    Course = course,
                    Categories = _context.Categories.ToList()
                });
            }
            return View(courseWithCate);
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
        // get all category
        [Authorize(Roles = "Staff")]
        public ActionResult AllCategory()
        {
            var allCategory = _context.Categories.ToList();
            return View(allCategory);
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
        //POST: /Admin/Edit Trainer
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
    }
}