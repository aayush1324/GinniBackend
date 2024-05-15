using Ginnis.Domains.DTOs;
using Ginnis.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IAddressRepository
    {
        Task<IActionResult> AddAddress(Guid userId, Address address);

        Task<IActionResult> GetAddresses(Guid userId);

        Task<IActionResult> EditAddress(Guid userId, Guid addressId, AddressDTO AddressDto);

        Task<IActionResult> DeleteAddress(Guid userId, Guid addressId);
    }

}
