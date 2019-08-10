using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Models
{
    public class Movies
    {
        public int MovieId { get; set; }
        public string Genre { get; set; }
        //TODO make this an int
        public DateTime Year { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public DateTime RunTime { get; set; }
    }
}
