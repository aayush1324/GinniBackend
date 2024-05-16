using Ginnis.Domains.Entities;
using Ginnis.Repos.Interfaces;
using Ginnis.Services.Context;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Repositories
{
    public class ZipCodeRepository : IZipCodeRepository
    {
        private readonly AppDbContext _authContext;

        public ZipCodeRepository(AppDbContext authContext)
        {
            _authContext = authContext;
        }

 

        public async Task AddZipCode(ZipCode zipCode)
        {
            await _authContext.ZipCodes.AddAsync(zipCode);
            await _authContext.SaveChangesAsync();
        }


        public async Task<List<ZipCode>> GetAllZipCodes()
        {
            return await _authContext.ZipCodes.ToListAsync();
        }


        public async Task UpdateZipCode(ZipCode updatedZipcode)
        {
            var zipcode = await _authContext.ZipCodes.FindAsync(updatedZipcode.Id);
            if (zipcode != null)
            {
                // Update properties
                zipcode.PinCode = updatedZipcode.PinCode;
                zipcode.Delivery = updatedZipcode.Delivery;
                zipcode.OfficeType = updatedZipcode.OfficeType;
                zipcode.OfficeName = updatedZipcode.OfficeName;
                zipcode.RegionName = updatedZipcode.RegionName;
                zipcode.DivisionName = updatedZipcode.DivisionName;
                zipcode.District = updatedZipcode.District;
                zipcode.State = updatedZipcode.State;

                await _authContext.SaveChangesAsync();
            }
        }


        public async Task<bool> CheckZipCodeExists(string pinCode)
        {
            return await _authContext.ZipCodes.AnyAsync(p => p.PinCode == pinCode);
        }


        public async Task DeleteZipCode(Guid zipcodeId)
        {
            var zipcode = await _authContext.ZipCodes.FindAsync(zipcodeId);
            if (zipcode != null)
            {
                _authContext.ZipCodes.Remove(zipcode);
                await _authContext.SaveChangesAsync();
            }
        }
    
    }

}
