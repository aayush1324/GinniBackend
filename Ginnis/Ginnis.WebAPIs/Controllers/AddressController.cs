using Ginnis.Domains.Entities;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddressController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public AddressController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        // POST api/<AddressController>
        [HttpPost("addAddress")]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            if (address == null)
                return BadRequest();

            await _authContext.Addresses.AddAsync(address);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Add Address Success!"
            });
        }


        [HttpGet("getAddress")]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _authContext .Addresses.ToListAsync();
            if (addresses == null || addresses.Count == 0)
            {
                return NotFound();
            }

            return Ok(new
            {
                Addresses = addresses
            });
        }



        [HttpDelete("deleteAddress/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            try
            {
                // Find the address by its unique identifier
                var address = await _authContext.Addresses.FindAsync(addressId);

                if (address == null)
                {
                    return NotFound(); // Address not found
                }

                // Remove the address from the context
                _authContext.Addresses.Remove(address);

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Delete Address Success!"
                }); // Address successfully deleted
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut("editAddress/{addressId}")]
        public async Task<IActionResult> EditAddress(Guid addressId, [FromBody] Address updatedAddress)
        {
            try
            {
                // Find the address by its unique identifier
                var address = await _authContext.Addresses.FindAsync(addressId);

                if (address == null)
                {
                    return NotFound(); // Address not found
                }

                // Update address properties with the new values
                // Update address properties with the new values
                address.FirstName = updatedAddress.FirstName;
                address.LastName = updatedAddress.LastName;
                address.Phone = updatedAddress.Phone;
                address.Address1 = updatedAddress.Address1;
                address.Address2 = updatedAddress.Address2;
                address.Pincode = updatedAddress.Pincode;
                address.City = updatedAddress.City;
                address.State = updatedAddress.State;
                address.Default = updatedAddress.Default;

                // Save changes to the database
                await _authContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Update Address Success!"
                }); // Address successfully updated
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
