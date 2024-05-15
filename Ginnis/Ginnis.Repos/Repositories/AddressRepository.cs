using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Collections;
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


        public async Task<IActionResult> AddAddress(Guid userId, Address address)
        {
            if (address == null)
                return new BadRequestResult();

            address.Created_at = DateTime.Now;
            address.UserId = userId;


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



        public async Task<IActionResult> GetAddresses(Guid userId)
        {
            try
            {
                var addresses = await _authContext.Addresses
                    .Where(a => a.UserId == userId && !a.isDeleted)
                    .Select(a => new AddressDTO
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Phone = a.Phone,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        Pincode = a.Pincode,
                        City = a.City,
                        State = a.State,
                        Default = a.Default
                    })
                    .ToListAsync();

                if (addresses == null || addresses.Count == 0)
                    // return new NotFoundResult();
                    return new OkObjectResult(new { Message = "No Address Found!" });

                return new OkObjectResult(addresses);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> EditAddress(Guid userId, Guid addressId, AddressDTO updatedAddressDTO)
        {
            try
            {
                var address = await _authContext.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId && !a.isDeleted);

                if (address == null)
                    return new NotFoundResult(); // Address not found or not belonging to the user

                // Update address properties with the new values from DTO
                address.FirstName = updatedAddressDTO.FirstName;
                address.LastName = updatedAddressDTO.LastName;
                address.Phone = updatedAddressDTO.Phone;
                address.Address1 = updatedAddressDTO.Address1;
                address.Address2 = updatedAddressDTO.Address2;
                address.Pincode = updatedAddressDTO.Pincode;
                address.City = updatedAddressDTO.City;
                address.State = updatedAddressDTO.State;
                address.Default = updatedAddressDTO.Default;

                // Set Modified_at to the current date and time
                address.Modified_at = DateTime.Now;

                await _authContext.SaveChangesAsync();
                return new OkObjectResult(new { Message = "Update Address Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }



        public async Task<IActionResult> DeleteAddress(Guid userId, Guid addressId)
        {
            try
            {
                var address = await _authContext.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

                if (address == null)
                    return new NotFoundResult(); // Address not found or not belonging to the user

                // Set Deleted_at to the current date and time
                address.Deleted_at = DateTime.Now;

                // Set IsDeleted to true
                address.isDeleted = true;

                // _authContext.Addresses.Remove(address);

                await _authContext.SaveChangesAsync();
                return new OkObjectResult(new { Message = "Delete Address Success!" });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

    }

}
