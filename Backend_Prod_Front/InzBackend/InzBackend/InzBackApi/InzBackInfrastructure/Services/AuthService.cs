using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InzBackCore.Domain;
using InzBackCore.Repositories;
using InzBackInfrastructure.DTO;

namespace InzBackInfrastructure.Services
{
    
    public class AuthService : IAuthService
    {

        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        public AuthService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }
        public async Task<User> Login(string userName, string password)
        {
            var user = await _authRepository.Login(userName, password);
            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            user.Role = "guest";
            var useR = await _authRepository.Register(user, password);
            return useR;
        }

        public async Task<bool> UserExist(string userName)
        {
            var result = await _authRepository.UserExist(userName);
            return result;
        }

        public async Task<IEnumerable<UserAccountDto>> GetAsyncAllUsersAccounts()   // pobiera liste userow
        {
            var usersAccounts = await _authRepository.AllUsersAccounts();
            return _mapper.Map<IEnumerable<UserAccountDto>>(usersAccounts);     // a tu zwraca juz przeksztalcona 
        }

        public async Task<UserAccountDto> GetOneUserAccount(int userId)
        {
            var account = await _authRepository.UserAccount(userId);
            if (account == null)
            {
                throw new Exception($"Account with this id: {userId} not exist");
            }
            return _mapper.Map<UserAccountDto>(account);
        }

        public async Task RemoveUserAccount(int userId)
        {
            int adminAccountCount = 0;
            var account = await _authRepository.UserAccount(userId);
            if (account == null)
            {
                throw new Exception($"Account with this id: {userId} not exist");
            }
            if( account.Role == "admin")
            {
                var allAccounts = await _authRepository.AllUsersAccounts();
                foreach (User elem in allAccounts)
                {
                    if (elem.Role == "admin") adminAccountCount++;
                }
                if (adminAccountCount < 2)
                {
                    throw new Exception($"You can't delete last admin account.");
                }
            }
            
            await _authRepository.DeleteAsyncUserAccount(account);
        }

        public async Task<User> UpdateAsyncUserAccounUserName(int userId, User user)
        {
            var usersAccounts = await _authRepository.AllUsersAccounts();
            user.Username = user.Username.ToLower();
            usersAccounts = usersAccounts.Where(x => x.Username == user.Username);
            if(usersAccounts.Count() > 0)
            {
                throw new Exception($"Account with this username exist");
            }
             // zmienia na male znaki wszystko
            var account = await _authRepository.UserAccount(userId);    // sprawdza czy istnieje takie konto
            if (account == null)
            {
                throw new Exception($"Account with this id: {userId} not exist");
            }
            account.Username = user.Username;
            var newAccount = await _authRepository.UpdateAsyncUserAccountUserName(userId, account);
            return newAccount;
        }

        public async Task<User> UpdateAsyncUserAccounPassword(int userId, string password)
        {
            var account = await _authRepository.UserAccount(userId);    // sprawdza czy istnieje takie konto
            if (account == null)
            {
                throw new Exception($"Account with this id: {userId} not exist");
            }
            var newAccount = await _authRepository.UpdateAsyncUserAccountPassword(userId, password);
            return newAccount;
        }
        public async Task<User> UpdateAcynsUserRole(int userId, string role)
        {
            var account = await _authRepository.UserAccount(userId);    // sprawdza czy istnieje takie konto
            if (account == null)
            {
                throw new Exception($"Account with this id: {userId} not exist");
            }
            role = role.ToLower();
            account.Role = role;
            var newAccount = await _authRepository.UpdateAsyncUserRole(account);
            return newAccount;
        }

    }
}
