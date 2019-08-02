using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public List<int> MovieId { get; set; }
    }
}
