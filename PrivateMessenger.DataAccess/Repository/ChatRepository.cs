using PrivateMessenger.DataAccess.Data;
using PrivateMessenger.DataAccess.Repository.IRepository;
using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessenger.DataAccess.Repository
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly ApplicationDbContext _db;

        public ChatRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Chat chat)
        {
            _db.Chats.Update(chat);
            // here update only necessary info, implement later
        }
    }
}
