#nullable enable
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;


namespace foodsnap_ai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IConfiguration _configuration;
        public FoodController(IFoodRepository foodRepository, IConfiguration configuration)
        {
            _foodRepository = foodRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// This Endpoint is used to upload pictures of food items for calorie processsing
        /// </summary>
        /// <param name="imageFile">Image File</param>
        /// <returns>Success/Failure response</returns>
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile, [FromQuery]int? weight)
        {
            if(imageFile == null ||  imageFile.Length == 0)
            {
                return BadRequest("File not found");
            }
            try
            {
                var filePath = Path.Combine("ImageUploads", imageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                string apiUrl = _configuration.GetConnectionString("ApiModelUrl");
                using (var client = new HttpClient())
                {
                    using(var form = new MultipartFormDataContent())
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        var imageContent = new ByteArrayContent(imageBytes);
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                        form.Add(imageContent, "image", Path.GetFileName(filePath));

                        HttpResponseMessage response = await client.PostAsync(apiUrl, form);

                        if (!response.IsSuccessStatusCode)
                        {
                            return StatusCode((int)response.StatusCode, new { Message = "Error calling AI model", Status = response.StatusCode });
                        }

                        FoodPrediction foodPrediction = await response.Content.ReadFromJsonAsync<FoodPrediction>();

                        var foodDetails = await _foodRepository.GetFoodDetails(foodPrediction.Prediction);

                        if( weight != null && weight > 0)
                        {
                            decimal foodCalorie = CalculateCalories((int)weight, foodDetails.Calories);
                            foodDetails.Calories = foodCalorie;
                        }

                        FoodPredictionDTO foodPredictionDTO = new FoodPredictionDTO
                        {
                            FoodName = foodPrediction.Prediction,
                            ConfidenceScore = foodPrediction.Confidence,
                            Calories = foodDetails.Calories,
                            FullPrediction = foodPrediction.FullPrediction
                        };

                        return Ok(foodPredictionDTO);
                         
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message ="Error saving file", Error = ex.Message });
            }
        }

        [HttpPost("GetCalories")]
        public async Task<IActionResult> GetCalories(string foodName)
        {
            var foodDetails = await _foodRepository.GetFoodDetails(foodName);

            var FoodDetailsDTO = new FoodInfoDTO
            {
                Name = foodDetails.Name,
                Calories = foodDetails.Calories,
                Weight = foodDetails.Weight
            };

            return Ok(FoodDetailsDTO);
        }

        private decimal CalculateCalories(int weight, decimal foodCalorie)
        {
            decimal calorieForOnegram = foodCalorie / 100;
            return weight * calorieForOnegram;
        }
    }
}
