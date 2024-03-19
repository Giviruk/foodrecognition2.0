using FoodRecognition.Database.Interfaces;

namespace FoodRecognition.Database.Entities;

public class Dish : IEntity
{
    public string Id { get; set; } = null!;
    public double Price { get; set; } 
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool Deleted { get; set; }
}