using AutoMapper;
using FoodRecognition.Core.Services.Dish.Models;
using FoodRecognition.Database;
using Microsoft.EntityFrameworkCore;

namespace FoodRecognition.Core.Services.Dish.Impl;

public class DishService : IDishService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;


    public DishService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<DishModel> List(List<string> dishNames)
    {
        var dishes = await _dbContext.Dishes.Where(x => dishNames.Any(y => y == x.Id)).ToListAsync();
        var abbreviatedDishes = dishes.Select(x => new DishAbbreviated()
        {
            Id = x.Id,
            Price = x.Price
        }).ToList();
        return new DishModel()
        {
            Dishes = abbreviatedDishes,
            Sum = abbreviatedDishes.Sum(x=>x.Price)
        };
    }
}