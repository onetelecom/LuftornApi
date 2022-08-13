
using BussinessLayer.Repositories;
using System;

namespace BussinessLayer.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {

        UserRepository UserRepository { get; }
       
        int Save();
      
    }
}
