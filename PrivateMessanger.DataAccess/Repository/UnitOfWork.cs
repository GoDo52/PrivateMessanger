using PrivateMessanger.DataAccess.Data;
using PrivateMessanger.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IUserRepository User {  get; private set; }
        public IMessageRepository Message {  get; private set; }
        public IChatRepository Chat {  get; private set; }
        public IUserChatRepository UserChat {  get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            Message = new MessageRepository(_db);
            Chat = new ChatRepository(_db);
            UserChat = new UserChatRepository(_db);
        }

        public async Task SaveAsync()
        {
            _db.SaveChangesAsync();
        }

        public async Task CompleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
