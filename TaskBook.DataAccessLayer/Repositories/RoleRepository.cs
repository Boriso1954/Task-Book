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
    public class RoleRepository//: IDisposable
    {
        //private TaskBookDbContext _db;
        //private RoleManager<TbRole> _roleManager;

        //public RoleRepository(TaskBookDbContext database)
        //{
        //    _db = database;
        //    _roleManager = new RoleManager<TbRole>(new RoleStore<TbRole>(_db));
        //}

        //public async Task<IdentityResult> CreateRole(TbRole role)
        //{
        //    var result = await _roleManager.CreateAsync(role);
        //    return result;
        //}

        //public async Task<TbRole> GetRoleAsync(string roleName)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    return role;
        //}

        //public TbRole GetRole(string roleName)
        //{
        //    var role = _roleManager.FindByName(roleName);
        //    return role;
        //}


        //public void Dispose()
        //{
        //    _db.Dispose();
        //    _roleManager.Dispose();
        //}
    }
}
