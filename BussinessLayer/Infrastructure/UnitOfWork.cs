



using BussinessLayer.Repositories;
using DataLayer.DB;

namespace BussinessLayer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _ctx;
        public UnitOfWork(AppDbContext ctx)
        {
            _ctx = ctx;
            _ctx.ChangeTracker.LazyLoadingEnabled = true;
        }


        public UserRepository UserRepository => new UserRepository(_ctx);
      

      


        public void Dispose()
        {
            _ctx.Dispose();
        }



        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
