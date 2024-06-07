using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ginnis.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressesController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [Authorize]
        [HttpPost("addAddress")]
        public async Task<IActionResult> AddAddress(Guid userId, [FromBody] Address address)
        {
            // Pass userId along with the address to the repository method
            return await _addressRepository.AddAddress(userId, address);
        }


        [Authorize]
        [HttpGet("getAddress/{userId}")]
        public async Task<IActionResult> GetAddresses(Guid userId)
        {
            return await _addressRepository.GetAddresses(userId);
        }


        [Authorize]
        [HttpPut("editAddress/{addressId}")]
        public async Task<IActionResult> EditAddress(Guid userId, Guid addressId, [FromBody] AddressDTO updatedAddress)
        {
            if (updatedAddress == null)
                return BadRequest();

            // Call the repository method to edit the address
            return await _addressRepository.EditAddress(userId, addressId, updatedAddress);
        }


        [Authorize]
        [HttpDelete("deleteAddress/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid userId, Guid addressId)
        {
            return await _addressRepository.DeleteAddress(userId, addressId);        
        }


    }
}
