using PrivateMessenger.DataAccess.Data;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Repository
{
    public class AdministrationRoleRepository : Repository<AdministrationRole>, IAdministrationRoleRepository
    {
        private readonly ApplicationDbContext _db;

        public AdministrationRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(AdministrationRole role)
        {
            _db.AdministrationRoles.Update(role);
        }
    }
}
