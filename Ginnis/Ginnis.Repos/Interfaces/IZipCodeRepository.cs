using Ginnis.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Repos.Interfaces
{
    public interface IZipCodeRepository
    {
        Task AddZipCode(ZipCode zipCode);

        Task<List<ZipCode>> GetAllZipCodes();

        Task<bool> CheckZipCodeExists(string pinCode);

        Task DeleteZipCode(Guid zipcodeId);

        Task UpdateZipCode(ZipCode updatedZipcode);
    }
}
