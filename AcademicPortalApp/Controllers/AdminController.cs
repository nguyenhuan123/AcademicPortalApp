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
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
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

        public AdminController()
            
        {
            _context = new ApplicationDbContext();
            _user = new ApplicationUser();
            _manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>( new ApplicationDbContext()));
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        //get all trainer by Discriminator Trainer
        public ActionResult AllTrainer()
        {
            var allTrainer = _context.Users.OfType<Trainer>().Include(t => t.Type).ToList();

            List<TrainerViewModel> trainerInfo = new List<TrainerViewModel>();

            foreach(var trainer in allTrainer)
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
        //get all staff
        [Authorize(Roles ="Admin")]
        public ActionResult AllStaff()
        {
            var allStaff = _context.Users.OfType<TrainingStaff>().ToList();
            List<StaffViewModel> staffInfo = new List<StaffViewModel>();
            foreach( var staff in allStaff)
            {
                staffInfo.Add(new StaffViewModel()
                {
                    Id = staff.Id,
                    Email = staff.Email,
                    StaffName = staff.StaffName
                }); ;
            }
        
            return View(staffInfo);
        }
        //
        // GET: /Admin/Create new staff
        [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateNewStaff()
        {
            return View();
        }

        //
        // POST: /Admin/Create new staff account
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateNewStaff(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new TrainingStaff { UserName = model.Email, Email = model.Email, StaffName = model.StaffName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Staff");
                    return RedirectToAction("AllStaff");
                }
                AddErrors(result);
            }

            return View(model);
        }
        // GET: /Admin/Edit staff
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStaff(string Id)
        {
            var findStaff = _context.Users.OfType<TrainingStaff>().SingleOrDefault(t => t.Id == Id);
            if (findStaff == null)
            {
                return HttpNotFound();
            }
            StaffViewModel model = new StaffViewModel()
            {
                Id = findStaff.Id,
                StaffName = findStaff.StaffName,
                Email = findStaff.Email
            };
            return View(model);
        }
        // POST: /Admin/Edit staff
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaff(StaffViewModel model)
        {

            var findStaff = _context.Users.OfType<TrainingStaff>().SingleOrDefault(t => t.Id == model.Id);
            findStaff.StaffName = model.StaffName;
            _context.SaveChanges();
            return RedirectToAction("AllStaff");
        }
        [Authorize(Roles = "Admin")]
        // Admin/Delete Staff
        public ActionResult DeleteStaff(string Id)
        {
            var findStaff = _context.Users.SingleOrDefault(t => t.Id == Id);
            if (findStaff == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(findStaff);
            _context.SaveChanges();
            return RedirectToAction("AllStaff");
        }

        // GET: /Admin/Create new trainer account
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult CreateNewTrainer()
        {
            CreateNewTrainerViewModel model = new CreateNewTrainerViewModel() {
                Types = _context.Types.ToList()
            };
            return View(model);
        }

        //
        // POST: /Admin/Create new trainer account
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewTrainer(CreateNewTrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Trainer { UserName = model.Email, Email = model.Email, WorkingPlace = model.WorkingPlace, TypeId = model.TypeId, TrainerName= model.TrainerName };
                
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var userRole = UserManager.AddToRole(user.Id, "Trainer");
                    return RedirectToAction("AllTrainer");
                }
                AddErrors(result);
            }

            return View(model);
        }
        //Get: /Admin/Edit Trainer
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult EditTrainer(string Id)
        {
            var findTrainer = _context.Users.OfType<Trainer>().Include(t => t.Type).SingleOrDefault(t => t.Id == Id);
            if (findTrainer == null)
            {
                return HttpNotFound();
            }
            CreateNewTrainerViewModel model = new CreateNewTrainerViewModel()
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
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrainer(CreateNewTrainerViewModel model)
        {
            
            var findTrainer = _context.Users.OfType<Trainer>().SingleOrDefault(t => t.Id == model.Id);
            findTrainer.TrainerName = model.TrainerName;
            findTrainer.TypeId = model.TypeId;
            findTrainer.WorkingPlace = model.WorkingPlace;
            _context.SaveChanges();
            return RedirectToAction("AllTrainer");
        }
        // Admin/Delete Trainer
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteTrainer(string Id)
        {
            var findTrainer = _context.Users.SingleOrDefault(t => t.Id == Id);
            if(findTrainer == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(findTrainer);
            _context.SaveChanges();
            return RedirectToAction("AllTrainer");
        }

    }
}