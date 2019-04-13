using InzBackCore.Domain;
using InzBackInfrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InzBackInfrastructure.Services
{
    public interface IAuthService
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExist(string userName);  // sprawdza po loginie czy taki user istnieje
        Task<IEnumerable<UserAccountDto>> GetAsyncAllUsersAccounts();
        Task<UserAccountDto> GetOneUserAccount(int userId);

        Task RemoveUserAccount(int userId);
        Task<User> UpdateAsyncUserAccounUserName(int userId, User user);
        Task<User> UpdateAsyncUserAccounPassword(int userId, string password);
        Task<User> UpdateAcynsUserRole(int userId, string role);

    }
}
