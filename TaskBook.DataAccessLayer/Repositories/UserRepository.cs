using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    public class UserRepository: IDisposable
    {
        private AuthDbContext _db;
        private UserManager<User> _userManager;

        public UserRepository()
        {
            _db = new AuthDbContext();
            _userManager = new UserManager<User>(new UserStore<User>(_db));
        }

        public async Task<IdentityResult> RegisterUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<User> GetUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public void Dispose()
        {
            _db.Dispose();
            _userManager.Dispose();

        }
    }
}
