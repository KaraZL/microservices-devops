using Series.Domain.Common;
using Series.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Domain.Entites
{
    public class SeriesModel : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public Origin CountryOrigin { get; set; } = new Origin(string.Empty, string.Empty);
    }
}
