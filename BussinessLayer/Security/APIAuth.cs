using BussinessLayer.Infrastructure;
using DataLayer.DTOs.UserDTOS;
using DataLayer.Entities;
using DataLayer.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BL.Security
{
    public interface IAuthenticateService
    {
        User AuthenticateUser(LoginDTO request, out string token);
    }
   
    public interface IUserManagementService
    {
        User IsValidUser(string username, string password);

    }
 
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUnitOfWork unitOfWork, IUserManagementService service, IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
            _unitOfWork = unitOfWork;
        }

        public User AuthenticateUser(LoginDTO request, out string token)
        {

            token = string.Empty;
            var user = _userManagementService.IsValidUser(request.Cred, request.Password);
            if (user != null)
            {
               
                List<Claim> ClaimList = new List<Claim>();
                ClaimList.Add(new Claim(ClaimTypes.Name, request.Cred));
                ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
              

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var expireDate = DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration);

                var tokenDiscriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(ClaimList),
                    Expires = expireDate,
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenObj = tokenHandler.CreateToken(tokenDiscriptor);
                token = tokenHandler.WriteToken(tokenObj);
            }
            return user;
        }
    }
    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _uow;
        public UserManagementService(IUnitOfWork uow) { _uow = uow; }

        public User IsValidUser(string cred, string password)
        {
            var user = _uow.UserRepository.GetMany(ent =>  ent.Email == cred&& ent.Password == EncryptANDDecrypt.EncryptText(password)).ToHashSet();
            return user.Count() == 1 ? user.FirstOrDefault() : null;
        }







    }
}
