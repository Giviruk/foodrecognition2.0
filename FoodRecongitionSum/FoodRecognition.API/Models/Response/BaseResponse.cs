namespace FoodRecognition.API.Models.Response;

public class BaseResponse<T>
{
    public BaseResponse()
    {
        Success = true;
    }

    public bool Success { get; set; }

    public T? Data { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    public void AddError(string message)
    {
        Success = false;
        if (Errors != null)
            Errors = Errors.Concat(new[] { message });
        else
            Errors = new[] { message };
    }
}
