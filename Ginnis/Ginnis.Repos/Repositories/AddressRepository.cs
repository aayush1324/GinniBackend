using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _authContext;

        public AddressRepository(AppDbContext context)
        {
            _authContext = context;
        }


        public async Task<IActionResult> AddAddress(Address address)
        {
            if (address == null)
                return new BadRequestResult();

            try
            {
                await _authContext.Addresses.AddAsync(address);
                await _authContext.SaveChangesAsync();
                return new OkObjectResult(new { Message = "Add Address Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                var addresses = await _authContext.Addresses.ToListAsync();
                if (addresses == null || addresses.Count == 0)
                    return new NotFoundResult();
                return new OkObjectResult(addresses);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            try
            {
                var address = await _authContext.Addresses.FindAsync(addressId);
                if (address == null)
                    return new NotFoundResult();
                _authContext.Addresses.Remove(address);
                await _authContext.SaveChangesAsync();
                return new OkObjectResult(new { Message = "Delete Address Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        public async Task<IActionResult> EditAddress(Guid addressId, Address updatedAddress)
        {
            try
            {
                var address = await _authContext.Addresses.FindAsync(addressId);
                if (address == null)
                    return new NotFoundResult();
                address.FirstName = updatedAddress.FirstName;
                address.LastName = updatedAddress.LastName;
                address.Phone = updatedAddress.Phone;
                address.Address1 = updatedAddress.Address1;
                address.Address2 = updatedAddress.Address2;
                address.Pincode = updatedAddress.Pincode;
                address.City = updatedAddress.City;
                address.State = updatedAddress.State;
                address.Default = updatedAddress.Default;
                await _authContext.SaveChangesAsync();
                return new OkObjectResult(new { Message = "Update Address Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }

}
