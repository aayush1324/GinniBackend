using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipCodesController : ControllerBase
    {
        private readonly IZipCodeRepository _zipcodeRepository;

        public ZipCodesController(IZipCodeRepository zipcodeRepository)
        {
            _zipcodeRepository = zipcodeRepository;
        }



        [HttpPost("addZipCode")]
        public async Task<IActionResult> AddZipCode([FromBody] ZipCode zipCode)
        {
            if (zipCode == null)
                return BadRequest();

            await _zipcodeRepository.AddZipCode(zipCode);

            return Ok(new { Message = "Add ZipCode Success!" });
        }



        [HttpGet("getAllZipCode")]
        public async Task<IActionResult> GetAllZipCode()
        {
            var zipcodelist = await _zipcodeRepository.GetAllZipCodes();

            if (zipcodelist == null || zipcodelist.Count == 0)
            {
                return NotFound();
            }

            return Ok(zipcodelist);
        }



        [HttpPost("checkZipCode")]
        public async Task<IActionResult> CheckZipcode([FromBody] ZipCode request)
        {
            var pinCodeExists = await _zipcodeRepository.CheckZipCodeExists(request.PinCode);

            return Ok(new { Available = pinCodeExists });
        }



        [HttpDelete("deleteZipCode/{zipcodeId}")]
        public async Task<IActionResult> DeleteZipCode(Guid zipcodeId)
        {
            await _zipcodeRepository.DeleteZipCode(zipcodeId);

            return Ok(new { Message = "Delete Zipcode Success!" });
        }



        [HttpPut("editZipCode/{zipcodeId}")]
        public async Task<IActionResult> EditZipCode(Guid zipcodeId, [FromBody] ZipCode updatedZipcode)
        {
            updatedZipcode.Id = zipcodeId; // Ensure the ID is set correctly

            await _zipcodeRepository.UpdateZipCode(updatedZipcode);

            return Ok(new { Message = "Update ZipCode Success!" });
        }
    }

}
