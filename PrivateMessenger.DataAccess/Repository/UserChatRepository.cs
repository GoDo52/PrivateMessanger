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
    public class UserChatRepository : Repository<UserChat>, IUserChatRepository
    {
        private readonly ApplicationDbContext _db;

        public UserChatRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(UserChat userChat)
        {
            _db.UserChats.Update(userChat);
            // here update only necessary info, like users that enter and leave chat, implement later (for group chats)
        }
    }
}
