using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.DtoModels
{
    public class AuthenticationDto
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
