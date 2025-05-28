using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

           var filePath = Path.Combine("ImageUploads", imageFile.FileName);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return Ok("Image uploaded successfully");
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
