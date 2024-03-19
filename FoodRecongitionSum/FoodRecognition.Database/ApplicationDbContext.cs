using FoodRecognition.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodRecognition.Database;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        //FoodRecognition.Database.Migrate();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Dish> Dishes { get; set; } = null!;
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}