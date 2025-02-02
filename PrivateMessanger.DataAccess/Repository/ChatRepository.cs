using PrivateMessanger.DataAccess.Data;
using PrivateMessanger.DataAccess.Repository.IRepository;
using PrivateMessanger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DataAccess.Repository
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly ApplicationDbContext _db;

        public ChatRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Chat chat)
        {
            _db.Chats.Update(chat);
            // here update only necessary info, implement later
        }
    }
}
