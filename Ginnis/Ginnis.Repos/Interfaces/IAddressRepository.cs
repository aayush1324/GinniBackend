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
        Task<IActionResult> AddAddress(Address address);
        Task<IActionResult> GetAddresses();
        Task<IActionResult> DeleteAddress(Guid addressId);
        Task<IActionResult> EditAddress(Guid addressId, Address updatedAddress);
    }

}
