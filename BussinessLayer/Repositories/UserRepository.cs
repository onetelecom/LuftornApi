
using BussinessLayer.Infrastructure;
using DataLayer.DB;
using DataLayer.Entities;
using System.Collections.Generic;

namespace BussinessLayer.Repositories
{
    public interface IUserRepository
    { }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext ctx) : base(ctx)
        { }

       
    }
}
