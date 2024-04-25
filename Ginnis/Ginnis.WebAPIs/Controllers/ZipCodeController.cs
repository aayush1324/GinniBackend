using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipCodeController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public ZipCodeController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }


        // POST api/<ZipCodeController>
        [HttpPost("addZipCode")]
        public async Task<IActionResult> AddZipCode([FromBody] ZipCode zipCode)
        {
            if (zipCode == null)
                return BadRequest();

            await _authContext.ZipCodes.AddAsync(zipCode);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Add ZipCode Success!"
            });
        }



        [HttpGet("getAllZipCode")]
        public async Task<IActionResult> GetAllZipCode()
        {
            var zipcodelist = await _authContext.ZipCodes.ToListAsync();

            if (zipcodelist == null || zipcodelist.Count == 0)
            {
                return NotFound();
            }

            return Ok(zipcodelist);
        }



        [HttpPost("checkZipCode")]
        public async Task<IActionResult> CheckZipcode([FromBody] ZipCode request)
        {
            var pinCodeExists = await _authContext.ZipCodes.AnyAsync(p => p.PinCode == request.PinCode);

            return Ok(new { Available = pinCodeExists });
        }



        [HttpDelete("deleteZipCode/{zipcodeId}")]
        public async Task<IActionResult> DeleteZipCode(Guid zipcodeId)
        {
            try
            {
                var zipcode = await _authContext.ZipCodes.FindAsync(zipcodeId);

                if (zipcode == null)
                {
                    return NotFound(); 
                }

                _authContext.ZipCodes.Remove(zipcode);
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Delete Zipcode Success!"
                }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("editZipCode/{zipcodeId}")]
        public async Task<IActionResult> EditZipCode(Guid zipcodeId, [FromBody] ZipCode updatedZipcode)
        {
            try
            {
                var zipcode = await _authContext.ZipCodes.FindAsync(zipcodeId);

                if (zipcode == null)
                {
                    return NotFound(); // Address not found
                }

                // Assuming 'zipcode' and 'updatedZipcode' are instances of the class containing these properties
                zipcode.PinCode = updatedZipcode.PinCode;
                zipcode.Delivery = updatedZipcode.Delivery;
                zipcode.OfficeType = updatedZipcode.OfficeType;
                zipcode.OfficeName = updatedZipcode.OfficeName;
                zipcode.RegionName = updatedZipcode.RegionName;
                zipcode.DivisionName = updatedZipcode.DivisionName;
                zipcode.District = updatedZipcode.District;
                zipcode.State = updatedZipcode.State;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Update ZipCode Success!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
