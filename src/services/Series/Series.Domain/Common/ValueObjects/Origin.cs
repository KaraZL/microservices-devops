using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Domain.Common.ValueObjects
{
    public class Origin : ValueObject
    {
        public string CountryName { get; set; }
        public string ZipCode { get; set; }

        public Origin(string countryName, string zipCode)
        {
            CountryName = countryName;
            ZipCode = zipCode;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CountryName;
            yield return ZipCode;
        }
    }
}
