using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }  

        public int Year { get; set; }
    }
}
