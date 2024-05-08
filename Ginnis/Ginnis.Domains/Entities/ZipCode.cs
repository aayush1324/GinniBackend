using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginnis.Domains.Entities
{
    public class ZipCode
    {
        public Guid Id { get; set; }

        public string PinCode { get; set; }


        public string Delivery { get; set; }

        public string OfficeType { get; set; }

        public string OfficeName { get; set; }

        public string RegionName { get; set; }

        public string DivisionName { get; set; }

        public string District { get; set; }

        public string State { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Modified_at { get; set; }

        public DateTime? Deleted_at { get; set; }
    }
}

