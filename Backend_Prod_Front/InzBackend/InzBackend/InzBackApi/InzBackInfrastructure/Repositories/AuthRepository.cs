using InzBackCore.Domain;
using InzBackCore.Repositories;
using InzBackInfrastructure.InzBackDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InzBackInfrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private InzBackContext _context;

        public AuthRepository(InzBackContext context)
        {
            _context = context;
        }
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);   // CHYBA to out ustawia te argumenty na zewnatrz czyli na te zadeklarowane nad metoda
            user.PasswordHash = passwordHash;   // a tu zakodowane wg klucza haslo
            user.PasswordSalt = passwordSalt;   // cyzli tu juz mamy do usera przypisany klucz klago kodowania uzywamy 
            await _context.Users.AddAsync(user);    // zapisujemy usera ktoremu baza przypisze id password przekazujemy jawnie oddzielnie, username tez jest przekazane 
            // a passwordhash i passwordsalt jest tutaj powyzej ogarniane na podstawie kodowania i podanego hasla
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())    // generuje obiekt uzywajac kodowanai hmacsha512 chyba
            {
                passwordSalt = hmac.Key;    // wyciaga z niego klucz kodowania chyba
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  // i zakodowuje w nim nasze haslo
            }
            
        }

        public async Task<User> Login(string userName, string password)
        {
            var user =  _context.Users.FirstOrDefault(x => x.Username == userName); // pobiera usera o takim loginie
            if(user == null)
            {
                return null;    // jezeli nie ma takiego to zwraca null
            }
                
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt));   // jak jest to w tej metodzie weryfikuje haslo

            return await Task.FromResult(user);   // i jak wszystko przejdzie to go zwraca

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)  // pobiera haslo wpisane na logowaniu 
            // i haslo usera o wpisanym loginie z bazy i klucz kodowania
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))    // generuje obiekt kodowania uzywajac klucza kodowania
                //dla wyszukanego w bazie usera
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  // i zakodowuje podane wpisane haslo przez powyzej juz podany klucz
                for(int i=0; i< computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;   // i sprawdza kazdy znak w ciagu iczy sa takie same
                }
                return true;
            }
        }

        public async Task<bool> UserExist(string userName)
        {
            if (_context.Users.Any(x => x.Username == userName))    // tu jak znajdzie usera o takiej nazwie to zwraca true
                return await Task.FromResult(true); // to moze byc blad ze jest ten return await na boolsach

            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<User>> AllUsersAccounts() // pobiera liste uzytkownikow
        {
            var usersAccountsList = from user in _context.Users select user;
            return await Task.FromResult(usersAccountsList);
        }

        public async Task<User> UserAccount(int userId)
        {
            var account = _context.Users.FirstOrDefault(x => x.Id == userId);
            return await Task.FromResult(account);
        }

    
        public async Task DeleteAsyncUserAccount(User user)
        {
            var matches = _context.Matches.Where(x => x.UserId == user.Id);
            List <Matchh> listameczy= matches.ToList();
            foreach(Matchh elem in listameczy)
            {
                _context.Matches.Remove(elem);
                _context.SaveChanges();
            }
            var players = _context.Players.Where(x => x.UserId == user.Id);
            List<Player> listazawodnikow = players.ToList();
            foreach (Player elem in listazawodnikow)
            {
                _context.Players.Remove(elem);
                _context.SaveChanges();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            await Task.CompletedTask;
        }
       

        public async Task<User> UpdateAsyncUserAccountUserName(int userId, User user)
        {   
            _context.Users.Update(user);
            _context.SaveChanges();
            return await Task.FromResult(user);
        }

        public async Task<User> UpdateAsyncUserAccountPassword(int userId, string password)
        {

            var account = await UserAccount(userId); // pobiera usera o takim id
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);   // CHYBA to out ustawia te argumenty na zewnatrz czyli na te zadeklarowane nad metoda
            account.PasswordHash = passwordHash;   // a tu zakodowane wg klucza haslo
            account.PasswordSalt = passwordSalt;   // cyzli tu juz mamy do usera przypisany klucz klago kodowania uzywamy 
            _context.Users.Update(account);
            _context.SaveChanges();
            return await Task.FromResult(account);
        }

        public async Task<User> UpdateAsyncUserRole(User account)
        {
            _context.Users.Update(account);
            _context.SaveChanges();
            return await Task.FromResult(account);
        }


    }
}
