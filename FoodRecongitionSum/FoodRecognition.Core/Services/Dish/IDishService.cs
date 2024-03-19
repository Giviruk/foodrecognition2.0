using FoodRecognition.Core.Services.Dish.Models;

namespace FoodRecognition.Core.Services.Dish;

public interface IDishService
{
    Task<DishModel> List(List<string> dishNames);
}