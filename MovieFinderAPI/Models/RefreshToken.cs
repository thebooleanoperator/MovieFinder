﻿using System;

namespace MovieFinder.Models
{
    public partial class RefreshToken
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public bool Invalidated { get; set; }
        public int UserId { get; set; }
    }
}
