using System;

namespace MovieFinder.Repository.BaseEntity
{
    public interface IBaseEntity
    {
        DateTime DateCreated { get; set; }
    }
}
