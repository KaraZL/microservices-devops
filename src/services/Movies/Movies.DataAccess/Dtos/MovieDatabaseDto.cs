using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Dtos
{
    public class MovieDatabaseDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
    }
}
