using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.UserDTOS
{
    public class LoginResponeDTO
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
