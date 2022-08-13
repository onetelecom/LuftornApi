using AutoMapper;
using BL.Security;
using BussinessLayer.Infrastructure;
using DataLayer.DTOs.UserDTOS;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Services.UserServices
{
    public interface IUserService
    {
        public LoginResponeDTO Login(LoginDTO DTO);
        public User AddUser(UserDTO DTO);
        public User UpdateUser(UpdateUserDTO DTO);
        public bool DeleteUser(Guid UserId);
        public bool DeleteAllUser();
        public User GetUserById(Guid UserId);
        public HashSet<User> GetAllUser();
        public LoginResponeDTO LoginSocialFaceBook(UserDTO DTO);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _authenticateService;

        public UserService(IUnitOfWork uow, IMapper mapper, IAuthenticateService authenticateService)
        {
            _uow = uow;
            _mapper = mapper;
            _authenticateService = authenticateService;
        }
        public User AddUser(UserDTO DTO)
        {
            try
            {
                var User = _mapper.Map<User>(DTO);
                User.Id=Guid.NewGuid();
                User.Password= EncryptANDDecrypt.EncryptText(DTO.Password);
                _uow.UserRepository.Add(User);
                _uow.Save();
                return User;
            }
            catch (Exception ex)
            {

                User user = new User();
                return user;
            }
            
        }
        public LoginResponeDTO LoginSocialFaceBook(UserDTO DTO)
        {
            try
            {
                var CheckUser = _uow.UserRepository.GetMany(a => a.Email == DTO.Email && a.Password == EncryptANDDecrypt.EncryptText(DTO.Password)).FirstOrDefault();
                if (CheckUser!=null)
                {
                    var TokenRegister = "";
                    var LoginDTORegister = new LoginDTO { Cred = CheckUser.Email, Password = DTO.Password };
                    var LoggedInUserRegister = _authenticateService.AuthenticateUser(LoginDTORegister, out TokenRegister);
                    return new LoginResponeDTO { Token = TokenRegister, User = CheckUser };
                }
                var User = _mapper.Map<User>(DTO);
                User.Id = Guid.NewGuid();
                User.Password = EncryptANDDecrypt.EncryptText(DTO.Password);
                _uow.UserRepository.Add(User);
                _uow.Save();
                var Token = "";
                var LoginDTO = new LoginDTO { Cred = User.Email, Password = DTO.Password };
                var LoggedInUser = _authenticateService.AuthenticateUser(LoginDTO, out Token);
                return new LoginResponeDTO { Token = Token, User = User };
            }
            catch (Exception ex)
            {

              
                return null;
            }

        }

        public bool DeleteAllUser()
        {
            foreach (var item in _uow.UserRepository.GetAll())
            {
                _uow.UserRepository.Delete(item.Id);

            }
            _uow.Save();
            return true;
        }

        public bool DeleteUser(Guid UserId)
        {
            try
            {
                _uow.UserRepository.Delete(UserId);
                _uow.Save();
                return true;
            }
            catch (Exception)
            {

                return false;

            }

        }

        public HashSet<User> GetAllUser()
        {
            return _uow.UserRepository.GetAll().ToHashSet();
        }

        public User GetUserById(Guid UserId)
        {
            return _uow.UserRepository.GetById(UserId);

        }

        public LoginResponeDTO Login(LoginDTO DTO)
        {
            var Token = "";
            var User = _authenticateService.AuthenticateUser(DTO, out Token);
            return new LoginResponeDTO {Token=Token,User=User };
        }

        public User UpdateUser(UpdateUserDTO DTO)
        {
            var User = _uow.UserRepository.GetById(DTO.Id);
            if (User!=null)
            {
                var MappedUser = _mapper.Map<User>(DTO);
                _uow.UserRepository.Update(MappedUser);
                _uow.Save();
                return MappedUser;
            }
            return User;
        }
    }
}
