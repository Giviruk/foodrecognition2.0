using Microsoft.AspNetCore.Mvc;

namespace FoodRecognitionGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodRecognitionController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public FoodRecognitionController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost]
    public async Task<IActionResult> Get(IFormFile image)
    {
        var sliceResult = await Slice(image);
        var predictResult = await Predict(sliceResult);
        var cheque = await GetSum(predictResult.Result);
        return Ok(cheque.Data);
    }

    private async Task<SumResult> GetSum(List<string> dishKeys)
    {
        var content = JsonContent.Create(new {dishNames = dishKeys});
        var response = await _httpClient.PostAsync("https://localhost:14300/api/Dishes/List", content);
        var result = await response.Content.ReadFromJsonAsync<SumResult>();
        return result;
    }

    private async Task<PythonResult> Predict(PythonResult pythonResult)
    {
        var content = new MultipartFormDataContent();
        var i = 1;
        foreach (var base64Str in pythonResult.Result)
        {
            var bytes = Convert.FromBase64String(base64Str);
            var byteContent = new ByteArrayContent(bytes);
            content.Add(byteContent, $"f{i}", $"f{i++}.jpg");
        }

        var responseMessage = await _httpClient.PostAsync("http://localhost:14100/predict", content);
        var result = await responseMessage.Content.ReadFromJsonAsync<PythonResult>();
        return result;
    }

    private async Task<PythonResult> Slice(IFormFile image)
    {
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        content.Add(streamContent, "files", image.FileName);
        var responseMessage = await _httpClient.PostAsync("http://localhost:14200/extract", content);
        var result = await responseMessage.Content.ReadFromJsonAsync<PythonResult>();
        return result;
    }

    private class PythonResult
    {
        public List<string> Result { get; set; }
    }

    private class SumResult
    {
        public bool Success { get; set; }
        public DishModel Data { get; set; }
        public List<string> Errors { get; set; }
    }

    public class DishModel
    {
        public List<DishAbbreviated> Dishes { get; set; } = null!;
        public double Sum { get; set; }

        public class DishAbbreviated
        {
            public string Id { get; set; } = null!;
            public double Price { get; set; }
        }
    }
}