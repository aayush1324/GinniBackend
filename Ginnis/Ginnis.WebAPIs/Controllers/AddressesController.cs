using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
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


        [HttpPost("addAddress")]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            return await _addressRepository.AddAddress(address);
        }



        [HttpGet("getAddress")]
        public async Task<IActionResult> GetAddresses()
        {
            return await _addressRepository.GetAddresses();
        }



        [HttpDelete("deleteAddress/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            return await _addressRepository.DeleteAddress(addressId);
        }



        [HttpPut("editAddress/{addressId}")]
        public async Task<IActionResult> EditAddress(Guid addressId, [FromBody] Address updatedAddress)
        {
            return await _addressRepository.EditAddress(addressId, updatedAddress);
        }
    }
}
