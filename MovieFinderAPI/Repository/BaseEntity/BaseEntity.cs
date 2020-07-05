using System;

namespace MovieFinder.Repository.BaseEntity
{
    public class BaseEntity : IBaseEntity
    {
        public DateTime DateCreated { get; set; }

        public BaseEntity()
        {
            DateCreated = DateTime.UtcNow;
        }
    }
}
