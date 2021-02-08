using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AcademicPortalApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Model8", throwIfV1Schema: false)
        {
        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<TraineeCourses> TraineeCourses { get; set; }
        public DbSet<TrainerCourses> TrainerCourses { get; set; }
        public DbSet<ProgrammingLanguages> ProgrammingLanguages { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }   
    }
}