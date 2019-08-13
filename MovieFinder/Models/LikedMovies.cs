using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Models
{
    public class LikedMovies
    {
        public int LikedId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
