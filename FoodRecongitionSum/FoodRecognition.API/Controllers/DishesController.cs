using FoodRecognition.API.Models.Request;
using FoodRecognition.API.Models.Response;
using FoodRecognition.Core.Services.Dish;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FoodRecognition.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishesController(IDishService dishService)
    {
        _dishService = dishService;
    }

    /// <summary>
    /// Получить список блюд с ценами и сумму 
    /// </summary>
    /// <param name="dishNames"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<object>>> List([FromBody] DishRequest dishNames)
    {
        var response = new BaseResponse<object>();
        if (!ModelState.IsValid) response.AddError("BadRequest, model state is not valid");
        try
        {
            response.Data = await _dishService.List(dishNames.DishNames);
        }
        catch (Exception e)
        {
            response.AddError(e.Message);
            Log.Error(e,
                $"{ControllerContext.ActionDescriptor.ControllerName}.{ControllerContext.ActionDescriptor.ActionName} Error");
        }

        return response.Success ? Ok(response) : BadRequest(response);
    }
}