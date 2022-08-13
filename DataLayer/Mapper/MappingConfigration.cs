



using AutoMapper;
using DataLayer.DTOs.UserDTOS;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mapper
{

    public class MappingConfigration : Profile
    {

        
        public MappingConfigration()
        {
        
           CreateMap<User, UserDTO>(MemberList.Source).ReverseMap();
           CreateMap<User, UpdateUserDTO>(MemberList.Source).ReverseMap();
          
        }
    }
}
