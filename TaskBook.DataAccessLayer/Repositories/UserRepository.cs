using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    public class UserRepository: IDisposable
    {
        private AuthDbContext _db;
        private UserManager<TbUser> _userManager;

        public UserRepository(AuthDbContext database)
        {
            _db = database;
            _userManager = new UserManager<TbUser>(new UserStore<TbUser>(_db));
        }

        public async Task<IdentityResult> CreateUser(TbUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<TbUser> GetUserAsync(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public TbUser GetUserByName(string userName)
        {
            var user = _userManager.FindByName(userName);
            return user;
        }

        public void Dispose()
        {
            _db.Dispose();
            _userManager.Dispose();

        }
    }
}
