using InzBackCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InzBackCore.Repositories
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExist(string userName);  // sprawdza po loginie czy taki user istnieje
        Task<IEnumerable<User>> AllUsersAccounts();
        Task<User> UserAccount(int userId);
        Task DeleteAsyncUserAccount(User user);
        Task<User> UpdateAsyncUserAccountUserName(int userId, User user);
        Task<User> UpdateAsyncUserAccountPassword(int userId, string password);
        Task<User> UpdateAsyncUserRole(User account);
    }
}
