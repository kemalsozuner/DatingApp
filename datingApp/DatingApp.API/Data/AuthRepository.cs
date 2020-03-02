using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext dataContext;
        public AuthRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

        }
        public async Task<User> Login(string username, string password)
        {
            var  user = await dataContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {   
                if (VerifyPassword(user,password)){
                    return user;    
                }
            }
            return null;
        }

        private bool VerifyPassword(User user,string enteredPassword)
        {
             using (var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
                {           
                   var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPassword)); 
                   for (int i=0;i<computedHash.Length;i++) {
                       if (computedHash[i] != user.PasswordHash[i])
                        return false;
                   }              

                }
                return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash,passwordSalt;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await dataContext.Users.AddAsync(user);
            await dataContext.SaveChangesAsync();
            return user;            
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));                
            }
        }

        public async Task<bool> UserExists(string username)
        {
             var  user = await dataContext.Users.FirstOrDefaultAsync(u => u.Username == username);
             return (user != null);
        }
    }
}