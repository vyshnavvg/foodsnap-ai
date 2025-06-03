using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace foodsnap_ai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        /// <summary>
        /// This Endpoint is used to upload pictures of food items for calorie processsing
        /// </summary>
        /// <param name="imageFile">Image File</param>
        /// <returns>Success/Failure response</returns>
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
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
                string apiUrl = "http://127.0.0.1:5000/detect-image";
                using (var client = new HttpClient())
                {
                    using(var form = new MultipartFormDataContent())
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        var imageContent = new ByteArrayContent(imageBytes);
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                        form.Add(imageContent, "image", Path.GetFileName(filePath));

                        HttpResponseMessage response = await client.PostAsync(apiUrl, form);
                        response.EnsureSuccessStatusCode();
                        string result = await response.Content.ReadAsStringAsync();
                        return Ok(result);
                         
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message ="Error saving file", Error = ex.Message });
            }
        }

        [HttpGet("GetCalories")]
        public async Task<IActionResult> GetCalories()
        {
            return Ok("Calories retrieved successfully.");
        }
    }
}
