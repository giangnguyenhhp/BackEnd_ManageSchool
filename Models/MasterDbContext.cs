using ASP_Web_API.Models.AuthModels;
using ASP_Web_API.Models.CoreModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_API.Models;

public class MasterDbContext : IdentityDbContext<User,Role,string>
{
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<School> Schools { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<Specialization> Specializations { get; set; }

    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName!=null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName[6..]);
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }
}