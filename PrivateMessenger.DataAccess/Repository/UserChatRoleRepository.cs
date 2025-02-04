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
    public class UserChatRoleRepository : Repository<UserChatRole>, IUserChatRoleRepository
    {
        private readonly ApplicationDbContext _db;

        public UserChatRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(UserChatRole chatRole)
        {
            _db.UserChatRoles.Update(chatRole);
        }
    }
}
