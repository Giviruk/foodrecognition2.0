using FoodRecognition.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodRecognition.Database;

public static class SeedData
{
    public static async Task EnsureSeedData(IServiceProvider provider)
    {
        var dbContext = provider.GetRequiredService<ApplicationDbContext>();
        var f = new StreamReader("dishes.txt");
        var rnd = new Random();
        string? s;
        var exitDishes = await dbContext.Dishes.ToListAsync();
        while ((s = await f.ReadLineAsync()) != null)
            if (exitDishes.All(x => x.Id != s))
                await dbContext.Dishes.AddAsync(new Dish()
                {
                    Created = DateTime.Now.ToUniversalTime(),
                    Deleted = false,
                    Id = s,
                    Modified = DateTime.Now.ToUniversalTime(),
                    Price = rnd.Next(100, 500)
                });

        await dbContext.SaveChangesAsync();
    }
}